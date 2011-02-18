CREATE OR REPLACE FUNCTION insert_test_tags(itemscount integer)
  RETURNS integer AS
$BODY$
declare 
  index integer;
begin
  for index  in 1..itemsCount loop
    insert into tags(name) values('tag' || index);
  end loop;
  return index;
end;
$BODY$
  LANGUAGE 'plpgsql' VOLATILE
  COST 100;