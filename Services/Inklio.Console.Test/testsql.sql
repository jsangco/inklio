-- drop table inklio.[ask_tag]
-- drop table inklio.[delivery_tag]
-- drop table inklio.[tag]
-- drop table inklio.[comment]
-- drop table inklio.[delivery]
-- drop table inklio.[ask]
-- drop table inklio.[user]

-- delete from inklio.[ask_tag] where ask_id > 0
-- delete from inklio.[delivery_tag] where delivery_id > 0
-- delete from inklio.[upvote] where id > 0
-- delete from inklio.[tag] where id > 0
-- delete from inklio.[comment] where id > 0
-- delete from inklio.[delivery] where id > 0
-- delete from inklio.[ask] where id > 0
-- delete from inklio.[user] where id > 0

select * from inklio.[upvote] where id > 0
select * from inklio.[ask_tag] where ask_id > 0
select * from inklio.[delivery_tag] where delivery_id > 0
select * from inklio.[tag] where id > 0
select * from inklio.[comment] where id > 0
select * from inklio.[delivery] where id > 0
select * from inklio.[ask] where id > 0
select * from inklio.[user] where id > 0