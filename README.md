# InMemeoryVsRedis .NET 7 >  Caching InMemoryCache vs REDIS

------------------------------------------------------------------------------------------------------------------------------------

Docker desktop indirdik. Docker ile REDIS (REmote DIctionary Server) kullanımı


Redis supports 6 data types. You need to know what type of value that a key maps to, as for each data type, the command to retrieve it is different.

Here are the commands to retrieve key value(s):

- if value is of type string -> GET <key>
- if value is of type hash -> HGET or HMGET or HGETALL <key>
- if value is of type lists -> lrange <key> <start> <end>
- if value is of type sets -> smembers <key>
- if value is of type sorted sets -> ZRANGEBYSCORE <key> <min> <max>
- if value is of type stream -> xread count <count> streams <key> <ID>. https://redis.io/commands/xread


- type <key>   > key in typeını almak için. her tipin komutları farklı olduğu için bilmek gerekli.  
- KEYS * > redisteki herşeyi getirir
- delete keys
	- FLUSHDB > OK : deletes the keys in a database
	- FLUSHALL > OK : deletes all keys in all databases
- select db
	select 1 > OK : rediste 15 db var, hangisiyle çalışmak istiyorsak onu seçebiliriz


------------------------------------------------------------------------------------------------------------------------------------


1. docker run --name some-redis -d redis > hub.docker.comdan redis image kurulumu yapıldı : ismi some redis 
2. docker ps >	tüm dosker imageleri listendi (örn container Id d234234242)
3. docker stop d23 > containerIdnin ilk 3 karakterini verip o containerı stop ettim -ki silebileyim (port maplicem)
4. docker rm d23 > ilgili container silindi : remove
5. docker run -p 6380:6379 --name some-redis -d redis > ile yeniden redis kuruldu, bu sefer senin def portun olan 6379 bendeki 6380e denk gelsin dedik. localhost:6380 diyebilmem için.
6. docker ps > tüm docker imageleri listendi (bu sefer örn container Id sd24234242)
7. docker exec -it sd2 sh > it :interactive sh shell > containera bağlandım
8. redis-cli > REDIS client terminali içine girdim.
9. ping yazım PONG aldm. redis clienta başarılı bir şekilde  bağlandım.


------------------------------------------------------------------------------------------------------------------------------------

- string veri tipi komutları - herşey serileştirilerek tutulabilir max 512MB

10. SET name merve > key:name value:merve ile veri attım. OK yanıtı aldım
11. GET name > "merve"
12. GETRANGE name 0 2 > "mer"
13. SET visitor 1000 > OK
14. GET visitor > "1000"
15. INCR visitor > (integer) 1001  : int +1 yaptı
16. GET visitor > "1001"
17. INCRBY visitor 10 > (integer) 1011
18. GET visitor > "1011"
19. DECR visitor > (integer) 1010
20. DECRBY visitor 20 > (integer) 990
21. GET visitor > "990"
22. APPEND name taya > (integer) 9
23. GET name > "mervetaya"
24. clear > sayfayı temizle
25. set name uğursaç > OK
26. get name > "u\xc4\x9fursa\xc3\xa7"
27. Ctrl+C > redis-cli içinden çıktım. 
28. redis-cli --raw > --raw ile yeniden başlattım. encode edilmemiş olan -ing dışındaki karakteri okuyabilek için encode edilmiş hale getiriyor
29.  get name > uğursaç

------------------------------------------------------------------------------------------------------------------------------------

- list veri tipi komutları - : C#.LinkedList  başa ve ya sona ekler ve siler

10. LPUSH books book1 > (integer) 1 > left push
11. LPUSH books book2 > (integer) 2 > left push  (book2, book1 oldu)
12. RPUSH books book3> (integer) 3 > right push (book2, book1, book3 oldu)
13. LPUSH books book4 > (integer) 4 > left push (book4, book2, book1, book3 oldu)
14. LRANGE books 0 -1 > 0. indexten hepsini getir
	1) "book4"
	2) "book2"
	3) "book1"
	4) "book3"
15. LRANGE books 0 2 > 0.indexten 2.index dahil getir
	1) "book4"
	2) "book2"
	3) "book1"
16. LPOP books 1 >  en baştan 1 tane sildi
	1) "book4"
17. LRANGE books 0 -1
	1) "book2"
	2) "book1"
	3) "book3"
18. RPOP books 2 > en sondan 2 tane sildi
	1) "book3"
	2) "book1"
19. LRANGE books 0 -1
	1) "book2"
20. RPUSH books book1 
	(integer) 2
21 - LRANGE books 0 -1
	1) "book2"
	2) "book1"
22. LINDEX books 1 > 1.indextekini ver
	"book1"
23. LINDEX books 0
	"book2"

------------------------------------------------------------------------------------------------------------------------------------

- SET veri tipi komutları - list gibi indexi var. datalar unique olmalı.  random eklenir data. başa ve ya sona ekle gibi karar belirleyemeyiz.

10. SADD colors blue > (integer) 1
11. SADD colors red > (integer) 1
12. SADD colors red > (integer) 0 -> datalar unique olmalı. 
13. SADD colors green > (integer) 1
14. SMEMBERS colors > sırası random
	1) "blue"
	2) "green"
	3) "red"
15. SREM colors red > (integer) 1
16. SMEMBERS colors
	1) "blue"
	2) "green"

------------------------------------------------------------------------------------------------------------------------------------

- SORTED SET veri tipi komutları - list gibi indexi var. datalar unique olmalı.  veriyi nereye ekleyebileceğimizi seçebiliyoruz : score üzerinden. score unique olomak zorunda değil

10. ZADD books 1 book1 > 1
11. ZADD books 5 book5 > 1
12. ZADD books 10 book10 > 1
13. ZADD books 2 book2 > 1
14. ZRANGE books 0 10
	book1
	book2
	book5
	book10
15. ZADD books 2 book22 > 1
16. ZRANGE books 0 -1
	book1
	book2
	book22
	book5
	book10
17.  ZRANGE books 0 -1 WITHSCORES
	book1
	1
	book2
	2
	book22
	2
	book5
	5
	book10
	10
18. ZREM books book22 > 1
19. ZRANGE books 0 -1
	book1
	book2
	book22
	book5
	book10

------------------------------------------------------------------------------------------------------------------------------------

- HASH veri tipi komutları - C#.dictionary 

10. HMSET dict pen kalem > OK
11. HMSET dict bag çanta > OK
12. HMSET dict book kitap > OK
13. HGET dict book > kitap
14. HDEL dict book > 1
15. HGETALL dict
	pen
	kalem
	bag
	çanta


------------------------------------------------------------------------------------------------------------------------------------

- stream kaldı
