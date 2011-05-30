--
-- PostgreSQL database dump
--

-- Started on 2011-05-30 13:00:53

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = off;
SET check_function_bodies = false;
SET client_min_messages = warning;
SET escape_string_warning = off;

--
-- TOC entry 350 (class 2612 OID 70357)
-- Name: plpgsql; Type: PROCEDURAL LANGUAGE; Schema: -; Owner: -
--

CREATE PROCEDURAL LANGUAGE plpgsql;


SET search_path = public, pg_catalog;

--
-- TOC entry 19 (class 1255 OID 70363)
-- Dependencies: 3 350
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
-- TOC entry 20 (class 1255 OID 70391)
-- Dependencies: 350 3
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
-- TOC entry 1538 (class 1259 OID 70539)
-- Dependencies: 1838 3
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
-- TOC entry 1539 (class 1259 OID 70547)
-- Dependencies: 3
-- Name: albums_genres; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE albums_genres (
    album_id integer NOT NULL,
    genre_id integer NOT NULL
);


--
-- TOC entry 1537 (class 1259 OID 70537)
-- Dependencies: 3 1538
-- Name: albums_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE albums_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1903 (class 0 OID 0)
-- Dependencies: 1537
-- Name: albums_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE albums_id_seq OWNED BY albums.id;


--
-- TOC entry 1541 (class 1259 OID 70552)
-- Dependencies: 1840 3
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
-- TOC entry 1542 (class 1259 OID 70560)
-- Dependencies: 3
-- Name: artists_albums; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE artists_albums (
    artist_id integer NOT NULL,
    album_id integer NOT NULL
);


--
-- TOC entry 1540 (class 1259 OID 70550)
-- Dependencies: 1541 3
-- Name: artists_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE artists_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1907 (class 0 OID 0)
-- Dependencies: 1540
-- Name: artists_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE artists_id_seq OWNED BY artists.id;


--
-- TOC entry 1544 (class 1259 OID 70565)
-- Dependencies: 3
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
-- TOC entry 1543 (class 1259 OID 70563)
-- Dependencies: 1544 3
-- Name: genres_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE genres_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1910 (class 0 OID 0)
-- Dependencies: 1543
-- Name: genres_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE genres_id_seq OWNED BY genres.id;


--
-- TOC entry 1546 (class 1259 OID 70574)
-- Dependencies: 3
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
-- TOC entry 1545 (class 1259 OID 70572)
-- Dependencies: 1546 3
-- Name: listens_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE listens_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1913 (class 0 OID 0)
-- Dependencies: 1545
-- Name: listens_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE listens_id_seq OWNED BY listens.id;


--
-- TOC entry 1548 (class 1259 OID 70583)
-- Dependencies: 3
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
-- TOC entry 1547 (class 1259 OID 70581)
-- Dependencies: 3 1548
-- Name: moods_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE moods_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1916 (class 0 OID 0)
-- Dependencies: 1547
-- Name: moods_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE moods_id_seq OWNED BY moods.id;


--
-- TOC entry 1550 (class 1259 OID 70592)
-- Dependencies: 3
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
-- TOC entry 1549 (class 1259 OID 70590)
-- Dependencies: 1550 3
-- Name: places_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE places_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1919 (class 0 OID 0)
-- Dependencies: 1549
-- Name: places_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE places_id_seq OWNED BY places.id;


--
-- TOC entry 1552 (class 1259 OID 70601)
-- Dependencies: 3
-- Name: tags; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags (
    id integer NOT NULL,
    name character varying NOT NULL,
    private_marks character varying,
    comments character varying,
    description character varying,
    create_date timestamp with time zone
);


--
-- TOC entry 1553 (class 1259 OID 70608)
-- Dependencies: 3
-- Name: tags_albums; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags_albums (
    tag_id integer NOT NULL,
    album_id integer NOT NULL
);


--
-- TOC entry 1554 (class 1259 OID 70611)
-- Dependencies: 3
-- Name: tags_artists; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE tags_artists (
    tag_id integer NOT NULL,
    artist_id integer NOT NULL
);


--
-- TOC entry 1551 (class 1259 OID 70599)
-- Dependencies: 1552 3
-- Name: tags_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE tags_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1924 (class 0 OID 0)
-- Dependencies: 1551
-- Name: tags_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE tags_id_seq OWNED BY tags.id;


--
-- TOC entry 1555 (class 1259 OID 70614)
-- Dependencies: 3
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
-- TOC entry 1557 (class 1259 OID 70622)
-- Dependencies: 3
-- Name: user_logins; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE user_logins (
    id integer NOT NULL,
    user_id integer NOT NULL,
    login_date timestamp without time zone NOT NULL
);


--
-- TOC entry 1556 (class 1259 OID 70620)
-- Dependencies: 3 1557
-- Name: user_logins_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE user_logins_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1928 (class 0 OID 0)
-- Dependencies: 1556
-- Name: user_logins_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE user_logins_id_seq OWNED BY user_logins.id;


--
-- TOC entry 1559 (class 1259 OID 70637)
-- Dependencies: 3
-- Name: users; Type: TABLE; Schema: public; Owner: -; Tablespace: 
--

CREATE TABLE users (
    id integer NOT NULL,
    user_name character varying NOT NULL,
    password character varying NOT NULL,
    settings xml
);


--
-- TOC entry 1558 (class 1259 OID 70635)
-- Dependencies: 3 1559
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MAXVALUE
    NO MINVALUE
    CACHE 1;


--
-- TOC entry 1931 (class 0 OID 0)
-- Dependencies: 1558
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE users_id_seq OWNED BY users.id;


--
-- TOC entry 1837 (class 2604 OID 70542)
-- Dependencies: 1538 1537 1538
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE albums ALTER COLUMN id SET DEFAULT nextval('albums_id_seq'::regclass);


--
-- TOC entry 1839 (class 2604 OID 70555)
-- Dependencies: 1541 1540 1541
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE artists ALTER COLUMN id SET DEFAULT nextval('artists_id_seq'::regclass);


--
-- TOC entry 1841 (class 2604 OID 70568)
-- Dependencies: 1544 1543 1544
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE genres ALTER COLUMN id SET DEFAULT nextval('genres_id_seq'::regclass);


--
-- TOC entry 1842 (class 2604 OID 70577)
-- Dependencies: 1546 1545 1546
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE listens ALTER COLUMN id SET DEFAULT nextval('listens_id_seq'::regclass);


--
-- TOC entry 1843 (class 2604 OID 70586)
-- Dependencies: 1548 1547 1548
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE moods ALTER COLUMN id SET DEFAULT nextval('moods_id_seq'::regclass);


--
-- TOC entry 1844 (class 2604 OID 70595)
-- Dependencies: 1549 1550 1550
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE places ALTER COLUMN id SET DEFAULT nextval('places_id_seq'::regclass);


--
-- TOC entry 1845 (class 2604 OID 70604)
-- Dependencies: 1551 1552 1552
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE tags ALTER COLUMN id SET DEFAULT nextval('tags_id_seq'::regclass);


--
-- TOC entry 1846 (class 2604 OID 70625)
-- Dependencies: 1557 1556 1557
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE user_logins ALTER COLUMN id SET DEFAULT nextval('user_logins_id_seq'::regclass);


--
-- TOC entry 1847 (class 2604 OID 70640)
-- Dependencies: 1559 1558 1559
-- Name: id; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE users ALTER COLUMN id SET DEFAULT nextval('users_id_seq'::regclass);


--
-- TOC entry 1849 (class 2606 OID 70945)
-- Dependencies: 1538 1538
-- Name: pk_albums; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY albums
    ADD CONSTRAINT pk_albums PRIMARY KEY (id);


--
-- TOC entry 1851 (class 2606 OID 70947)
-- Dependencies: 1541 1541
-- Name: pk_artists; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY artists
    ADD CONSTRAINT pk_artists PRIMARY KEY (id);


--
-- TOC entry 1855 (class 2606 OID 70949)
-- Dependencies: 1544 1544
-- Name: pk_genres; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY genres
    ADD CONSTRAINT pk_genres PRIMARY KEY (id);


--
-- TOC entry 1860 (class 2606 OID 70951)
-- Dependencies: 1546 1546
-- Name: pk_listens; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT pk_listens PRIMARY KEY (id);


--
-- TOC entry 1862 (class 2606 OID 70953)
-- Dependencies: 1548 1548
-- Name: pk_moods; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY moods
    ADD CONSTRAINT pk_moods PRIMARY KEY (id);


--
-- TOC entry 1866 (class 2606 OID 70955)
-- Dependencies: 1550 1550
-- Name: pk_places; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY places
    ADD CONSTRAINT pk_places PRIMARY KEY (id);


--
-- TOC entry 1870 (class 2606 OID 70957)
-- Dependencies: 1552 1552
-- Name: pk_tags; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tags
    ADD CONSTRAINT pk_tags PRIMARY KEY (id);


--
-- TOC entry 1874 (class 2606 OID 70959)
-- Dependencies: 1555 1555
-- Name: pk_tracks; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tracks
    ADD CONSTRAINT pk_tracks PRIMARY KEY (id);


--
-- TOC entry 1876 (class 2606 OID 70961)
-- Dependencies: 1557 1557
-- Name: pk_user_logins; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY user_logins
    ADD CONSTRAINT pk_user_logins PRIMARY KEY (id);


--
-- TOC entry 1878 (class 2606 OID 70965)
-- Dependencies: 1559 1559
-- Name: pk_users; Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY users
    ADD CONSTRAINT pk_users PRIMARY KEY (id);


--
-- TOC entry 1853 (class 2606 OID 70967)
-- Dependencies: 1541 1541
-- Name: uq_artists(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY artists
    ADD CONSTRAINT "uq_artists(name)" UNIQUE (name);


--
-- TOC entry 1857 (class 2606 OID 70969)
-- Dependencies: 1544 1544
-- Name: uq_genres(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY genres
    ADD CONSTRAINT "uq_genres(name)" UNIQUE (name);


--
-- TOC entry 1864 (class 2606 OID 70971)
-- Dependencies: 1548 1548
-- Name: uq_moods(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY moods
    ADD CONSTRAINT "uq_moods(name)" UNIQUE (name);


--
-- TOC entry 1868 (class 2606 OID 70973)
-- Dependencies: 1550 1550
-- Name: uq_places(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY places
    ADD CONSTRAINT "uq_places(name)" UNIQUE (name);


--
-- TOC entry 1872 (class 2606 OID 70975)
-- Dependencies: 1552 1552
-- Name: uq_tags(name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tags
    ADD CONSTRAINT "uq_tags(name)" UNIQUE (name);


--
-- TOC entry 1880 (class 2606 OID 70977)
-- Dependencies: 1559 1559
-- Name: uq_users(user_name); Type: CONSTRAINT; Schema: public; Owner: -; Tablespace: 
--

ALTER TABLE ONLY users
    ADD CONSTRAINT "uq_users(user_name)" UNIQUE (user_name);


--
-- TOC entry 1858 (class 1259 OID 70979)
-- Dependencies: 1546
-- Name: fki_listens_places; Type: INDEX; Schema: public; Owner: -; Tablespace: 
--

CREATE INDEX fki_listens_places ON listens USING btree (place_id);


--
-- TOC entry 1881 (class 2606 OID 70980)
-- Dependencies: 1539 1848 1538
-- Name: albums_genres_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY albums_genres
    ADD CONSTRAINT albums_genres_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1882 (class 2606 OID 70985)
-- Dependencies: 1539 1854 1544
-- Name: albums_genres_to_genres; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY albums_genres
    ADD CONSTRAINT albums_genres_to_genres FOREIGN KEY (genre_id) REFERENCES genres(id);


--
-- TOC entry 1883 (class 2606 OID 70990)
-- Dependencies: 1848 1542 1538
-- Name: fk_artists_albums_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY artists_albums
    ADD CONSTRAINT fk_artists_albums_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1884 (class 2606 OID 70995)
-- Dependencies: 1850 1541 1542
-- Name: fk_artists_albums_to_artists; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY artists_albums
    ADD CONSTRAINT fk_artists_albums_to_artists FOREIGN KEY (artist_id) REFERENCES artists(id);


--
-- TOC entry 1885 (class 2606 OID 71000)
-- Dependencies: 1546 1538 1848
-- Name: fk_listens_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1886 (class 2606 OID 71005)
-- Dependencies: 1548 1861 1546
-- Name: fk_listens_moods; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_moods FOREIGN KEY (mood_id) REFERENCES moods(id);


--
-- TOC entry 1887 (class 2606 OID 71010)
-- Dependencies: 1865 1546 1550
-- Name: fk_listens_places; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY listens
    ADD CONSTRAINT fk_listens_places FOREIGN KEY (place_id) REFERENCES places(id);


--
-- TOC entry 1888 (class 2606 OID 71015)
-- Dependencies: 1848 1553 1538
-- Name: fk_tags_albums_to_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_albums
    ADD CONSTRAINT fk_tags_albums_to_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1889 (class 2606 OID 71020)
-- Dependencies: 1553 1552 1869
-- Name: fk_tags_albums_to_tags; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_albums
    ADD CONSTRAINT fk_tags_albums_to_tags FOREIGN KEY (tag_id) REFERENCES tags(id);


--
-- TOC entry 1890 (class 2606 OID 71025)
-- Dependencies: 1850 1554 1541
-- Name: fk_tags_artists_to_artists; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_artists
    ADD CONSTRAINT fk_tags_artists_to_artists FOREIGN KEY (artist_id) REFERENCES artists(id);


--
-- TOC entry 1891 (class 2606 OID 71030)
-- Dependencies: 1554 1552 1869
-- Name: fk_tags_artists_to_tags; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tags_artists
    ADD CONSTRAINT fk_tags_artists_to_tags FOREIGN KEY (tag_id) REFERENCES tags(id);


--
-- TOC entry 1892 (class 2606 OID 71035)
-- Dependencies: 1538 1848 1555
-- Name: fk_tracks_albums; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY tracks
    ADD CONSTRAINT fk_tracks_albums FOREIGN KEY (album_id) REFERENCES albums(id);


--
-- TOC entry 1893 (class 2606 OID 71040)
-- Dependencies: 1559 1557 1877
-- Name: fk_user_logins_users; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY user_logins
    ADD CONSTRAINT fk_user_logins_users FOREIGN KEY (user_id) REFERENCES users(id);


--
-- TOC entry 1898 (class 0 OID 0)
-- Dependencies: 3
-- Name: public; Type: ACL; Schema: -; Owner: -
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- TOC entry 1899 (class 0 OID 0)
-- Dependencies: 19
-- Name: find_missing_genres(xml); Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON FUNCTION find_missing_genres(genresxml xml) FROM PUBLIC;
REVOKE ALL ON FUNCTION find_missing_genres(genresxml xml) FROM postgres;
GRANT ALL ON FUNCTION find_missing_genres(genresxml xml) TO postgres;
GRANT ALL ON FUNCTION find_missing_genres(genresxml xml) TO PUBLIC;
GRANT ALL ON FUNCTION find_missing_genres(genresxml xml) TO "user";


--
-- TOC entry 1900 (class 0 OID 0)
-- Dependencies: 20
-- Name: import_media(character varying); Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON FUNCTION import_media(xmlstr character varying) FROM PUBLIC;
REVOKE ALL ON FUNCTION import_media(xmlstr character varying) FROM postgres;
GRANT ALL ON FUNCTION import_media(xmlstr character varying) TO postgres;
GRANT ALL ON FUNCTION import_media(xmlstr character varying) TO PUBLIC;
GRANT ALL ON FUNCTION import_media(xmlstr character varying) TO "user";


--
-- TOC entry 1901 (class 0 OID 0)
-- Dependencies: 1538
-- Name: albums; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE albums FROM PUBLIC;
REVOKE ALL ON TABLE albums FROM postgres;
GRANT ALL ON TABLE albums TO postgres;
GRANT ALL ON TABLE albums TO "user";


--
-- TOC entry 1902 (class 0 OID 0)
-- Dependencies: 1539
-- Name: albums_genres; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE albums_genres FROM PUBLIC;
REVOKE ALL ON TABLE albums_genres FROM postgres;
GRANT ALL ON TABLE albums_genres TO postgres;
GRANT ALL ON TABLE albums_genres TO "user";


--
-- TOC entry 1904 (class 0 OID 0)
-- Dependencies: 1537
-- Name: albums_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE albums_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE albums_id_seq FROM postgres;
GRANT ALL ON SEQUENCE albums_id_seq TO postgres;
GRANT ALL ON SEQUENCE albums_id_seq TO "user";


--
-- TOC entry 1905 (class 0 OID 0)
-- Dependencies: 1541
-- Name: artists; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE artists FROM PUBLIC;
REVOKE ALL ON TABLE artists FROM postgres;
GRANT ALL ON TABLE artists TO postgres;
GRANT ALL ON TABLE artists TO "user";


--
-- TOC entry 1906 (class 0 OID 0)
-- Dependencies: 1542
-- Name: artists_albums; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE artists_albums FROM PUBLIC;
REVOKE ALL ON TABLE artists_albums FROM postgres;
GRANT ALL ON TABLE artists_albums TO postgres;
GRANT ALL ON TABLE artists_albums TO "user";


--
-- TOC entry 1908 (class 0 OID 0)
-- Dependencies: 1540
-- Name: artists_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE artists_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE artists_id_seq FROM postgres;
GRANT ALL ON SEQUENCE artists_id_seq TO postgres;
GRANT ALL ON SEQUENCE artists_id_seq TO "user";


--
-- TOC entry 1909 (class 0 OID 0)
-- Dependencies: 1544
-- Name: genres; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE genres FROM PUBLIC;
REVOKE ALL ON TABLE genres FROM postgres;
GRANT ALL ON TABLE genres TO postgres;
GRANT ALL ON TABLE genres TO "user";


--
-- TOC entry 1911 (class 0 OID 0)
-- Dependencies: 1543
-- Name: genres_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE genres_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE genres_id_seq FROM postgres;
GRANT ALL ON SEQUENCE genres_id_seq TO postgres;
GRANT ALL ON SEQUENCE genres_id_seq TO "user";


--
-- TOC entry 1912 (class 0 OID 0)
-- Dependencies: 1546
-- Name: listens; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE listens FROM PUBLIC;
REVOKE ALL ON TABLE listens FROM postgres;
GRANT ALL ON TABLE listens TO postgres;
GRANT ALL ON TABLE listens TO "user";


--
-- TOC entry 1914 (class 0 OID 0)
-- Dependencies: 1545
-- Name: listens_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE listens_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE listens_id_seq FROM postgres;
GRANT ALL ON SEQUENCE listens_id_seq TO postgres;
GRANT ALL ON SEQUENCE listens_id_seq TO "user";


--
-- TOC entry 1915 (class 0 OID 0)
-- Dependencies: 1548
-- Name: moods; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE moods FROM PUBLIC;
REVOKE ALL ON TABLE moods FROM postgres;
GRANT ALL ON TABLE moods TO postgres;
GRANT ALL ON TABLE moods TO "user";


--
-- TOC entry 1917 (class 0 OID 0)
-- Dependencies: 1547
-- Name: moods_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE moods_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE moods_id_seq FROM postgres;
GRANT ALL ON SEQUENCE moods_id_seq TO postgres;
GRANT ALL ON SEQUENCE moods_id_seq TO "user";


--
-- TOC entry 1918 (class 0 OID 0)
-- Dependencies: 1550
-- Name: places; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE places FROM PUBLIC;
REVOKE ALL ON TABLE places FROM postgres;
GRANT ALL ON TABLE places TO postgres;
GRANT ALL ON TABLE places TO "user";


--
-- TOC entry 1920 (class 0 OID 0)
-- Dependencies: 1549
-- Name: places_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE places_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE places_id_seq FROM postgres;
GRANT ALL ON SEQUENCE places_id_seq TO postgres;
GRANT ALL ON SEQUENCE places_id_seq TO "user";


--
-- TOC entry 1921 (class 0 OID 0)
-- Dependencies: 1552
-- Name: tags; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE tags FROM PUBLIC;
REVOKE ALL ON TABLE tags FROM postgres;
GRANT ALL ON TABLE tags TO postgres;
GRANT ALL ON TABLE tags TO "user";


--
-- TOC entry 1922 (class 0 OID 0)
-- Dependencies: 1553
-- Name: tags_albums; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE tags_albums FROM PUBLIC;
REVOKE ALL ON TABLE tags_albums FROM postgres;
GRANT ALL ON TABLE tags_albums TO postgres;
GRANT ALL ON TABLE tags_albums TO "user";


--
-- TOC entry 1923 (class 0 OID 0)
-- Dependencies: 1554
-- Name: tags_artists; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE tags_artists FROM PUBLIC;
REVOKE ALL ON TABLE tags_artists FROM postgres;
GRANT ALL ON TABLE tags_artists TO postgres;
GRANT ALL ON TABLE tags_artists TO "user";


--
-- TOC entry 1925 (class 0 OID 0)
-- Dependencies: 1551
-- Name: tags_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE tags_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE tags_id_seq FROM postgres;
GRANT ALL ON SEQUENCE tags_id_seq TO postgres;
GRANT ALL ON SEQUENCE tags_id_seq TO "user";


--
-- TOC entry 1926 (class 0 OID 0)
-- Dependencies: 1555
-- Name: tracks; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE tracks FROM PUBLIC;
REVOKE ALL ON TABLE tracks FROM postgres;
GRANT ALL ON TABLE tracks TO postgres;
GRANT ALL ON TABLE tracks TO "user";


--
-- TOC entry 1927 (class 0 OID 0)
-- Dependencies: 1557
-- Name: user_logins; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE user_logins FROM PUBLIC;
REVOKE ALL ON TABLE user_logins FROM postgres;
GRANT ALL ON TABLE user_logins TO postgres;
GRANT ALL ON TABLE user_logins TO "user";


--
-- TOC entry 1929 (class 0 OID 0)
-- Dependencies: 1556
-- Name: user_logins_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE user_logins_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE user_logins_id_seq FROM postgres;
GRANT ALL ON SEQUENCE user_logins_id_seq TO postgres;
GRANT ALL ON SEQUENCE user_logins_id_seq TO "user";


--
-- TOC entry 1930 (class 0 OID 0)
-- Dependencies: 1559
-- Name: users; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON TABLE users FROM PUBLIC;
REVOKE ALL ON TABLE users FROM postgres;
GRANT ALL ON TABLE users TO postgres;
GRANT ALL ON TABLE users TO "user";


--
-- TOC entry 1932 (class 0 OID 0)
-- Dependencies: 1558
-- Name: users_id_seq; Type: ACL; Schema: public; Owner: -
--

REVOKE ALL ON SEQUENCE users_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE users_id_seq FROM postgres;
GRANT ALL ON SEQUENCE users_id_seq TO postgres;
GRANT ALL ON SEQUENCE users_id_seq TO "user";


-- Completed on 2011-05-30 13:00:53

--
-- PostgreSQL database dump complete
--

