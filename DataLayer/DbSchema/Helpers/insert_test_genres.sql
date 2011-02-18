-- Function: insert_test_genres(integer)

-- DROP FUNCTION insert_test_genres(integer);

CREATE OR REPLACE FUNCTION insert_test_genres(itemscount integer)
  RETURNS integer AS
$BODY$
declare 
  index integer;
begin
  for index  in 1..itemsCount loop
    insert into genres(name) values('genre' || index);
  end loop;
  return index;
end;
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;

