CREATE OR REPLACE FUNCTION insert_test_data()
  RETURNS integer AS
$BODY$
declare 
  tagsCount integer;
  genresCount integer;
  artistsCount integer;
  albumsCount integer;
  tagsPerArtist integer;
  tracksPerAlbum integer;
  artistsPerAlbum integer;
  tagsPerAlbum integer;
  genresPerAlbum integer;
  index integer;
  i2 integer;
  displayNotification boolean;
begin
  tagsCount := 500;
  genresCount := 500;

  artistsCount := 1000;
  tagsPerArtist := 50;
  
  -- 20 albums per artist average
  albumsCount := artistsCount * 20; 
  tagsPerAlbum := 50;
  tracksPerAlbum := 25;
  artistsPerAlbum := 3;
  genresPerAlbum := 15;

  truncate table albums cascade;
  truncate table artists cascade;
  truncate table tags cascade;
  truncate table genres cascade;
  truncate table tracks cascade;

  raise notice 'Preparing sequences';

  perform SETVAL('tags_id_seq', 0);
  perform SETVAL('artists_id_seq', 0);
  perform SETVAL('albums_id_seq', 0);
  perform SETVAL('genres_id_seq', 0);
  perform SETVAL('tracks_id_seq', 0);
  

  raise notice 'Inserting tags...';

  for index in 1..tagsCount loop
    insert into tags(name) values('tag' || index);
  end loop;

  raise notice 'Inserting genres...';

  for index in 1..tagsCount loop
    insert into genres(name) values('genre' || index);
  end loop;

  -- inserting artists

  for index  in 1..artistsCount loop
    if index % 100 = 0 then
      displayNotification := true;
    else
      displayNotification = false;
    end if;

    if displayNotification then
      raise notice 'Inserting artist : % of %',index,artistsCount;
    end if;
    
    insert into artists(name,private_marks,biography) values('Artist' || index, 'Some marks' || index, 'Some biography' || index);

    --insert tags for artist
    for i2 in 1..tagsPerArtist loop
      insert into tags_artists(tag_id,artist_id) values(i2,index);
    end loop;
  end loop;

  -- inserting albums

  for index in 1..albumsCount loop
    if index % 100 = 0 then
      displayNotification := true;
    else
      displayNotification = false;
    end if;   

    if displayNotification then
      raise notice 'Inserting album : % of %. Percentage done: [%]',
			index,albumsCount,CAST(index AS FLOAT)/CAST(albumsCount AS FLOAT)*100;
    end if;

    insert into albums(name,rating) values('album' || index,1);

    --insert tags for albums
    for i2 in 1..tagsPerAlbum loop
      insert into tags_albums(tag_id,album_id) values(i2,index);
    end loop;    

    --insert genres for albums
    for i2 in 1..genresPerAlbum loop
      insert into albums_genres(album_id,genre_id) values(index,i2);
    end loop; 
       
    --insert tracks for albums
    for i2 in 1..tracksPerAlbum loop
      insert into tracks(name,track_index,album_id) values('track' || i2,i2, index);
    end loop;     

    --insert artists for albums
    for i2 in 1..artistsPerAlbum loop
      insert into artists_albums(artist_id,album_id) values(i2,index);
    end loop;
  end loop;
  
  return index;
end;
$BODY$
  LANGUAGE 'plpgsql' VOLATILE