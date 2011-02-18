-- Function: insert_test_artists(integer)

-- DROP FUNCTION insert_test_artists(integer);

-- call "select insert_test_artists(100000)"

CREATE OR REPLACE FUNCTION insert_test_artists(itemscount integer)
  RETURNS integer AS
$BODY$
declare 
  index integer;
begin
  for index  in 1..itemsCount loop
    insert into artists(name,private_marks,biography) values('Artist' || index, 'Some marks' || index, 'Some biography' || index);
  end loop;
  return index;
end;
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;

