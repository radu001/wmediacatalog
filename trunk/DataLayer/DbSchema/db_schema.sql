--
-- PostgreSQL database dump
--

-- Dumped from database version 9.0.4
-- Dumped by pg_dump version 9.0.4
-- Started on 2011-09-09 19:54:48

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

--
-- TOC entry 359 (class 2612 OID 11574)
-- Name: plpgsql; Type: PROCEDURAL LANGUAGE; Schema: -; Owner: -
--

CREATE OR REPLACE PROCEDURAL LANGUAGE plpgsql;


SET search_path = public, pg_catalog;

--
-- TOC entry 357 (class 1247 OID 16812)
-- Dependencies: 6 1575
-- Name: tagged_entity; Type: TYPE; Schema: public; Owner: -
--

CREATE TYPE tagged_entity AS (
	entity_id integer,
	entity_name character varying,
	entity_type integer
);


--
-- TOC entry 18 (class 1255 OID 16599)
-- Dependencies: 6 359
-- Name: find_missing_genres(xml); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION find_missing_genres(genresxml xml) RETURNS xml
    LANGUAGE plpgsql
    AS $$
    declare
    xmlStr xml;
    missingGenresXml text;
    arrayLength integer;
    xelement xml;
    list text[];
    begin
    list := (select * from (select xpath('/genres/g/text()',genresXml)::text[] x) as x);
    arrayLength := (select array_upper(list, 1));
    missingGenresXml := '
      ';

      FOR i IN 1..arrayLength LOOP
      if ((select count(1) from genres where name like list[i]) = 0) then
      --RAISE NOTICE 'array[i][1]=% not found in db', list[i];
      xelement := (select xmlelement(name g, text (list[i])));
      missingGenresXml := missingGenresXml || xelement;
      end if;
      END LOOP;

      missingGenresXml := missingGenresXml || '
    ';

    return missingGenresXml;

    end; $$;


--
-- TOC entry 19 (class 1255 OID 16822)
-- Dependencies: 357 359 6
-- Name: get_tagged_entities(character varying, integer, integer); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION get_tagged_entities(tagids character varying, ioffset integer, ilimit integer) RETURNS SETOF tagged_entity
    LANGUAGE plpgsql
    AS $$
declare
  cdelim text;
  trimmedIdsStr text;
  idList int[];
begin
  cdelim = ',';
  trimmedIdsStr = btrim(tagIDs, ',');
  idList = string_to_array(trimmedIdsStr, cdelim)::int[];

  return query select distinct on(entity_id,entity_type) entity_id, entity_name, entity_type from tagged_objects
    where tag_id in (select unnest(idList))
    offset ioffset limit ilimit;
end;
$$;


--
-- TOC entry 20 (class 1255 OID 16824)
-- Dependencies: 6 359
-- Name: get_tagged_entities_count(character varying); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION get_tagged_entities_count(tagids character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
  cdelim text;
  trimmedIdsStr text;
  idList int[];
  result integer;
begin
  cdelim = ',';
  trimmedIdsStr = btrim(tagIDs, ',');
  idList = string_to_array(trimmedIdsStr, cdelim)::int[];

  --select count(1) from (select distinct on(entity_id,entity_type) * from tagged_objects where tag_id in (select unnest(ARRAY[1,2]))) t
  return (select count(1) from (select distinct on(entity_id,entity_type) * from tagged_objects where tag_id in (select unnest(idList))) t);
end;
$$;


--
-- TOC entry 21 (class 1255 OID 16600)
-- Dependencies: 359 6
-- Name: import_media(character varying); Type: FUNCTION; Schema: public; Owner: -
--

CREATE FUNCTION import_media(xmlstr character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
 xml_data xml;
 albumsCount integer;
 genresCount integer;
 artistsCount integer;
 artistList text[];
 albumList text[];
 genreList text[];
 i int; j int; k int;
 artistID int; albumID int;
 xpathQuery character varying;
 currentAlbum character varying; currentArtist character varying;
 currentGenre character varying;
 albumYear int;
 genreID int;
begin
 xml_data = xmlStr::xml;
 --process all genres and ensure all of them are persisted into db
 genreList := (select xpath('//artists/a/al/gn/g/@name',xml_data)::text[] x);

 genresCount := array_upper(genreList,1);
 if genresCount > 0 then
	 --loop throigh unique genre names
	 for currentGenre in ( select distinct lower(a) from unnest(genreList) a) loop
	   --make first chars of genre words capital
	   insert into genres (name) select initcap(currentGenre)
	     where not exists (select g.name from genres g where
		g.name=initcap(currentGenre));
	 end loop;
 end if;


 artistList := (select xpath('//artists/a/@name',xml_data)::text[] x);

 artistsCount := array_upper(artistList,1);
 if artistsCount > 0 then
	 for i in 1..artistsCount loop
	   currentArtist := artistList[i];

	   --get or create new artist by name and fetch it's id
	    artistID = (select id from artists a where lower(a.name) =
		trim(lower(currentArtist)));
	    if artistID is null then
	      insert into artists(name) values(trim(currentArtist)) returning	id into artistID;
	    end if;

	   --process albums for given artist
	   xpathQuery := '//artists/a[@name="' || currentArtist || '"]/al/@name';
	   albumList := (select xpath(xpathQuery,xml_data)::text[] y);
           if ( array_upper(albumList,1) > 0 ) then
		   for j in 1..array_upper(albumList,1) loop
		     currentAlbum := albumList[j];

		     --check whether album with given name exists in database
		     albumsCount = (select count(1) from albums a where a.name = currentAlbum);
		     if ( albumsCount = 0 ) then
		       --get album year
		       xpathQuery := '//artists/a[@name="' || currentArtist || '"]/al[@name="' || currentAlbum || '"]/@year';
		       albumYear := (select y[1] from (select xpath(xpathQuery,xml_data)::text[] y) as y);

		       if ( albumYear is null ) then
			 albumYear := 1900;
		       end if;

		       --create new album and link with artist
		       insert into albums(name,rating,year) values(trim(currentAlbum),0,to_date('01 01 ' || albumYear, 'DD MM YYYY'))
			 returning id into albumID;
		     else
		       albumID := (select a.id from albums a where a.name = currentAlbum);
		     end if;

		     --check whether album is already bound to artists
		     albumsCount := (select count(1) from artists_albums where artist_id = artistID and album_id = albumID);
		     if ( albumsCount = 0 ) then
		       insert into artists_albums(artist_id,album_id) values(artistID,albumID);
		     end if;

		     --process genres of given album
		     xpathQuery := '//artists/a[@name="' || currentArtist ||
			'"]/al[@name="' || currentAlbum || '"]/gn/g/@name';
		     genreList := (select xpath(xpathQuery,xml_data)::text[] z);

		     genresCount := array_upper(genreList,1);
		     if genresCount > 0 then
		       for k in 1..genresCount loop
			 currentGenre := genreList[k];
			 genreID := (select id from genres g where g.name = currentGenre );
			 if genreID is not null then
			   if ( select count(1) from albums_genres where album_id = albumID and genre_id = genreID ) = 0 then
			     insert into albums_genres(album_id,genre_id) values(albumID, genreID);
			   end if;
			 end if;
		       end loop;
		     end if;
		   end loop;
	  end if;
	 end loop;
 end if;

 return 0;
end;
$$;


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 1551 (class 1259 OID 16601)
-- Dependencies: 1854 6
-- Name: albums; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE albums (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    private_marks character varying,
    label character varying(128),
    asin character varying,
    freedb_id character varying,
    year date,
    discs_count integer,
    rating integer NOT NULL,
    is_waste boolean DEFAULT false NOT NULL
);


--
-- TOC entry 1552 (class 1259 OID 16608)
-- Dependencies: 6
-- Name: albums_genres; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE albums_genres (
    album_id integer NOT NULL,
    genre_id integer NOT NULL
);


--
-- TOC entry 1553 (class 1259 OID 16611)
-- Dependencies: 6 1551
-- Name: albums_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE albums_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1915 (class 0 OID 0)
-- Dependencies: 1553
-- Name: albums_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE albums_id_seq OWNED BY albums.id;


--
-- TOC entry 1554 (class 1259 OID 16613)
-- Dependencies: 1856 6
-- Name: artists; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE artists (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    biography character varying,
    is_waste boolean DEFAULT false NOT NULL
);


--
-- TOC entry 1555 (class 1259 OID 16620)
-- Dependencies: 6
-- Name: artists_albums; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE artists_albums (
    artist_id integer NOT NULL,
    album_id integer NOT NULL
);


--
-- TOC entry 1556 (class 1259 OID 16623)
-- Dependencies: 6 1554
-- Name: artists_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE artists_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1916 (class 0 OID 0)
-- Dependencies: 1556
-- Name: artists_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE artists_id_seq OWNED BY artists.id;


--
-- TOC entry 1557 (class 1259 OID 16625)
-- Dependencies: 6
-- Name: genres; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE genres (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    comments character varying,
    description character varying
);


--
-- TOC entry 1558 (class 1259 OID 16631)
-- Dependencies: 6 1557
-- Name: genres_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE genres_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1917 (class 0 OID 0)
-- Dependencies: 1558
-- Name: genres_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE genres_id_seq OWNED BY genres.id;


--
-- TOC entry 1559 (class 1259 OID 16633)
-- Dependencies: 6
-- Name: listens; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE listens (
    id integer NOT NULL,
    date timestamp with time zone NOT NULL,
    review character varying,
    private_marks character varying,
    comments character varying,
    mood_id integer NOT NULL,
    album_id integer NOT NULL,
    listen_rating integer,
    place_id integer NOT NULL
);


--
-- TOC entry 1560 (class 1259 OID 16639)
-- Dependencies: 1559 6
-- Name: listens_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE listens_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1918 (class 0 OID 0)
-- Dependencies: 1560
-- Name: listens_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE listens_id_seq OWNED BY listens.id;


--
-- TOC entry 1561 (class 1259 OID 16641)
-- Dependencies: 6
-- Name: moods; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE moods (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    comment character varying,
    description character varying
);


--
-- TOC entry 1562 (class 1259 OID 16647)
-- Dependencies: 1561 6
-- Name: moods_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE moods_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1919 (class 0 OID 0)
-- Dependencies: 1562
-- Name: moods_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE moods_id_seq OWNED BY moods.id;


--
-- TOC entry 1563 (class 1259 OID 16649)
-- Dependencies: 6
-- Name: places; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE places (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    comment character varying,
    description character varying
);


--
-- TOC entry 1564 (class 1259 OID 16655)
-- Dependencies: 1563 6
-- Name: places_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE places_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1920 (class 0 OID 0)
-- Dependencies: 1564
-- Name: places_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE places_id_seq OWNED BY places.id;


--
-- TOC entry 1565 (class 1259 OID 16657)
-- Dependencies: 6
-- Name: tags; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    comments character varying,
    description character varying,
    create_date timestamp with time zone,
    color character varying NOT NULL,
    text_color character varying NOT NULL
);


--
-- TOC entry 1566 (class 1259 OID 16663)
-- Dependencies: 6
-- Name: tags_albums; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags_albums (
    tag_id integer NOT NULL,
    album_id integer NOT NULL
);


--
-- TOC entry 1567 (class 1259 OID 16666)
-- Dependencies: 6
-- Name: tags_artists; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags_artists (
    tag_id integer NOT NULL,
    artist_id integer NOT NULL
);


--
-- TOC entry 1574 (class 1259 OID 16806)
-- Dependencies: 1662 6
-- Name: tagged_objects; Type: VIEW; Schema: public; Owner: -
--

CREATE VIEW tagged_objects AS
    SELECT tar.tag_id, tar.artist_id AS entity_id, 0 AS entity_type, t.name AS tag_name, t.color AS tag_color, t.text_color AS tag_text_color, ar.name AS entity_name FROM tags_artists tar, tags t, artists ar WHERE ((tar.tag_id = t.id) AND (tar.artist_id = ar.id)) UNION ALL SELECT tal.tag_id, tal.album_id AS entity_id, 1 AS entity_type, t.name AS tag_name, t.color AS tag_color, t.text_color AS tag_text_color, al.name AS entity_name FROM tags_albums tal, tags t, albums al WHERE ((tal.tag_id = t.id) AND (tal.album_id = al.id));


--
-- TOC entry 1568 (class 1259 OID 16673)
-- Dependencies: 6 1565
-- Name: tags_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE tags_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1921 (class 0 OID 0)
-- Dependencies: 1568
-- Name: tags_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE tags_id_seq OWNED BY tags.id;


--
-- TOC entry 1569 (class 1259 OID 16675)
-- Dependencies: 6
-- Name: tracks; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tracks (
    id integer NOT NULL,
    name character varying NOT NULL,
    track_index integer,
    length integer,
    album_id integer NOT NULL
);


--
-- TOC entry 1570 (class 1259 OID 16681)
-- Dependencies: 6
-- Name: user_logins; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE user_logins (
    id integer NOT NULL,
    user_id integer NOT NULL,
    login_date timestamp without time zone NOT NULL
);


--
-- TOC entry 1571 (class 1259 OID 16684)
-- Dependencies: 6 1570
-- Name: user_logins_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE user_logins_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1922 (class 0 OID 0)
-- Dependencies: 1571
-- Name: user_logins_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE user_logins_id_seq OWNED BY user_logins.id;


--
-- TOC entry 1572 (class 1259 OID 16686)
-- Dependencies: 6
-- Name: users; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE users (
    id integer NOT NULL,
    user_name character varying NOT NULL,
    password character varying NOT NULL,
    settings text
);


--
-- TOC entry 1573 (class 1259 OID 16692)
-- Dependencies: 6 1572
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 1923 (class 0 OID 0)
-- Dependencies: 1573
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE users_id_seq OWNED BY users.id;


--
-- TOC entry 1855 (class 2604 OID 16694)
-- Dependencies: 1553 1551
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE albums ALTER COLUMN id SET DEFAULT nextval('albums_id_seq'::regclass);


--
-- TOC entry 1857 (class 2604 OID 16695)
-- Dependencies: 1556 1554
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE artists ALTER COLUMN id SET DEFAULT nextval('artists_id_seq'::regclass);


--
-- TOC entry 1858 (class 2604 OID 16696)
-- Dependencies: 1558 1557
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE genres ALTER COLUMN id SET DEFAULT nextval('genres_id_seq'::regclass);


--
-- TOC entry 1859 (class 2604 OID 16697)
-- Dependencies: 1560 1559
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE listens ALTER COLUMN id SET DEFAULT nextval('listens_id_seq'::regclass);


--
-- TOC entry 1860 (class 2604 OID 16698)
-- Dependencies: 1562 1561
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE moods ALTER COLUMN id SET DEFAULT nextval('moods_id_seq'::regclass);


--
-- TOC entry 1861 (class 2604 OID 16699)
-- Dependencies: 1564 1563
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE places ALTER COLUMN id SET DEFAULT nextval('places_id_seq'::regclass);


--
-- TOC entry 1862 (class 2604 OID 16700)
-- Dependencies: 1568 1565
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE tags ALTER COLUMN id SET DEFAULT nextval('tags_id_seq'::regclass);


--
-- TOC entry 1863 (class 2604 OID 16701)
-- Dependencies: 1571 1570
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE user_logins ALTER COLUMN id SET DEFAULT nextval('user_logins_id_seq'::regclass);


--
-- TOC entry 1864 (class 2604 OID 16702)
-- Dependencies: 1573 1572
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE users ALTER COLUMN id SET DEFAULT nextval('users_id_seq'::regclass);


--
-- TOC entry 1866 (class 2606 OID 16704)
-- Dependencies: 1551 1551
-- Name: pk_albums; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY albums
    ADD CONSTRAINT pk_albums PRIMARY KEY (id);


--
-- TOC entry 1868 (class 2606 OID 16706)
-- Dependencies: 1554 1554
-- Name: pk_artists; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY artists
    ADD CONSTRAINT pk_artists PRIMARY KEY (id);


--
-- TOC entry 1872 (class 2606 OID 16708)
-- Dependencies: 1557 1557
-- Name: pk_genres; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY genres
    ADD CONSTRAINT pk_genres PRIMARY KEY (id);


--
-- TOC entry 1877 (class 2606 OID 16710)
-- Dependencies: 1559 1559
-- Name: pk_listens; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT pk_listens PRIMARY KEY (id);


--
-- TOC entry 1879 (class 2606 OID 16712)
-- Dependencies: 1561 1561
-- Name: pk_moods; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY moods
    ADD CONSTRAINT pk_moods PRIMARY KEY (id);


--
-- TOC entry 1883 (class 2606 OID 16714)
-- Dependencies: 1563 1563
-- Name: pk_places; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY places
    ADD CONSTRAINT pk_places PRIMARY KEY (id);


--
-- TOC entry 1887 (class 2606 OID 16716)
-- Dependencies: 1565 1565
-- Name: pk_tags; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tags
    ADD CONSTRAINT pk_tags PRIMARY KEY (id);


--
-- TOC entry 1891 (class 2606 OID 16718)
-- Dependencies: 1569 1569
-- Name: pk_tracks; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tracks
    ADD CONSTRAINT pk_tracks PRIMARY KEY (id);


--
-- TOC entry 1893 (class 2606 OID 16720)
-- Dependencies: 1570 1570
-- Name: pk_user_logins; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY user_logins
    ADD CONSTRAINT pk_user_logins PRIMARY KEY (id);


--
-- TOC entry 1895 (class 2606 OID 16722)
-- Dependencies: 1572 1572
-- Name: pk_users; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY users
    ADD CONSTRAINT pk_users PRIMARY KEY (id);


--
-- TOC entry 1870 (class 2606 OID 16724)
-- Dependencies: 1554 1554
-- Name: uq_artists(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY artists
    ADD CONSTRAINT "uq_artists(name)" UNIQUE (name);


--
-- TOC entry 1874 (class 2606 OID 16726)
-- Dependencies: 1557 1557
-- Name: uq_genres(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY genres
    ADD CONSTRAINT "uq_genres(name)" UNIQUE (name);


--
-- TOC entry 1881 (class 2606 OID 16728)
-- Dependencies: 1561 1561
-- Name: uq_moods(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY moods
    ADD CONSTRAINT "uq_moods(name)" UNIQUE (name);


--
-- TOC entry 1885 (class 2606 OID 16730)
-- Dependencies: 1563 1563
-- Name: uq_places(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY places
    ADD CONSTRAINT "uq_places(name)" UNIQUE (name);


--
-- TOC entry 1889 (class 2606 OID 16732)
-- Dependencies: 1565 1565
-- Name: uq_tags(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tags
    ADD CONSTRAINT "uq_tags(name)" UNIQUE (name);


--
-- TOC entry 1897 (class 2606 OID 16734)
-- Dependencies: 1572 1572
-- Name: uq_users(user_name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY users
    ADD CONSTRAINT "uq_users(user_name)" UNIQUE (user_name);


--
-- TOC entry 1875 (class 1259 OID 16735)
-- Dependencies: 1559
-- Name: fki_listens_places; Type: INDEX; Schema: public; Owner: -; Tablespace: 
--

CREATE INDEX fki_listens_places ON listens USING btree (place_id);


--
-- TOC entry 1898 (class 2606 OID 16736)
-- Dependencies: 1552 1865 1551
-- Name: albums_genres_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY albums_genres
    ADD CONSTRAINT albums_genres_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1899 (class 2606 OID 16741)
-- Dependencies: 1871 1552 1557
-- Name: albums_genres_to_genres; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY albums_genres
    ADD CONSTRAINT albums_genres_to_genres FOREIGN KEY (genre_id) REFERENCES genres(id);


--
-- TOC entry 1900 (class 2606 OID 16746)
-- Dependencies: 1555 1551 1865
-- Name: fk_artists_albums_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY artists_albums
    ADD CONSTRAINT fk_artists_albums_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1901 (class 2606 OID 16751)
-- Dependencies: 1555 1867 1554
-- Name: fk_artists_albums_to_artists; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY artists_albums
    ADD CONSTRAINT fk_artists_albums_to_artists FOREIGN KEY (artist_id) REFERENCES artists(id);


--
-- TOC entry 1902 (class 2606 OID 16756)
-- Dependencies: 1865 1551 1559
-- Name: fk_listens_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1903 (class 2606 OID 16761)
-- Dependencies: 1559 1878 1561
-- Name: fk_listens_moods; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_moods FOREIGN KEY (mood_id) REFERENCES moods(id);


--
-- TOC entry 1904 (class 2606 OID 16766)
-- Dependencies: 1882 1559 1563
-- Name: fk_listens_places; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_places FOREIGN KEY (place_id) REFERENCES places(id);


--
-- TOC entry 1905 (class 2606 OID 16771)
-- Dependencies: 1865 1566 1551
-- Name: fk_tags_albums_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_albums
    ADD CONSTRAINT fk_tags_albums_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1906 (class 2606 OID 16776)
-- Dependencies: 1565 1886 1566
-- Name: fk_tags_albums_to_tags; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_albums
    ADD CONSTRAINT fk_tags_albums_to_tags FOREIGN KEY (tag_id) REFERENCES tags(id);


--
-- TOC entry 1907 (class 2606 OID 16781)
-- Dependencies: 1867 1567 1554
-- Name: fk_tags_artists_to_artists; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_artists
    ADD CONSTRAINT fk_tags_artists_to_artists FOREIGN KEY (artist_id) REFERENCES artists(id);


--
-- TOC entry 1908 (class 2606 OID 16786)
-- Dependencies: 1565 1886 1567
-- Name: fk_tags_artists_to_tags; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_artists
    ADD CONSTRAINT fk_tags_artists_to_tags FOREIGN KEY (tag_id) REFERENCES tags(id);


--
-- TOC entry 1909 (class 2606 OID 16791)
-- Dependencies: 1551 1569 1865
-- Name: fk_tracks_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tracks
    ADD CONSTRAINT fk_tracks_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1910 (class 2606 OID 16796)
-- Dependencies: 1570 1572 1894
-- Name: fk_user_logins_users; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY user_logins
    ADD CONSTRAINT fk_user_logins_users FOREIGN KEY (user_id) REFERENCES users(id);


-- Completed on 2011-09-09 19:54:49

--
-- PostgreSQL database dump complete
--

