var db = db.getSiblingDB("demo");
var password = "HY0Xg3z9/kqaY/xf+YU5FA==";  // P@ssw0rd

var user1Id = ObjectId("5aa3a6c360d53c0e5565f3a9");
var user2Id = ObjectId("5aa3a6c360d53c0e5565f3aa");
var addUsers = ()=> {
    db.user.drop();
    db.user.createIndex({'uname':1});
    db.user.insert( [
    {
        "_id": user1Id,
        "un": "antonio.dimotta@gmail.com",
        "pw": password,
        "fn": "Antonio",
        "ln": "Di Motta",
        "en": true,
        "au": ["admin","reader","writer"]
    },
    {
        "_id": user2Id,
        "un": "pippo@mail.com",
        "pw": password,
        "fn": "Pippo",
        "ln": "Pluto",
        "en": true,
        "au": ["reader","writer"]
    }
 ] )
};
addUsers();

var obj1Id = ObjectId();
var obj2Id = ObjectId();
var obj3Id = ObjectId();
var obj4Id = ObjectId();
var addData = ()=> {
    db.geodata.drop();
    db.geodata.createIndex({ "lo": "2dsphere" })
    db.geodata.insert( [
    {
        "_id": obj1Id,
        "lo": {
            "type": "Point",
            "coordinates": [14.283069,40.848082]
         },
        "cn": "Mate naples site 2",
        "nm": "Mate2",
        "ct": "text/plain",
        "tg": ["company"],
        "ui" : user1Id
    },
    {
        "_id": obj2Id,
        "lo": {
            "type": "Point",
            "coordinates": [14.274900,40.850506]
         },
        "cn": "Hotel Ramada",
        "nm": "Hotel",
        "ct": "text/plain",
        "tg": ["hotel"],
        "ui" : user1Id
    },
    {
        "_id": obj3Id,
        "lo": {
            "type": "Point",
            "coordinates": [14.271879,40.852815]
         },
        "cn": "Naples train station",
        "nm": "Station",
        "ct": "text/plain",
        "tg": ["city"],
        "ui" : user1Id
    },
    {
        "_id": obj4Id,
        "lo": {
            "type": "Point",
            "coordinates": [14.774473,40.681562]
         },
        "cn": "Mate naples site 1",
        "nm": "Mate1",
        "ct": "text/plain",
        "tg": ["company"],
        "ui" : user1Id
    },
 ] )
};
addData();