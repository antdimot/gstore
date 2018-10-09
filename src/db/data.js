var db = db.getSiblingDB("gstore");
var password = "HY0Xg3z9/kqaY/xf+YU5FA==";  // P@ssw0rd

var user1Id = ObjectId("5aa3a6c360d53c0e5565f3a9");
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
    }
 ] )
};
addUsers();

var obj1Id = ObjectId();
var addData = ()=> {
    db.geodata.drop();
    db.geodata.createIndex({ "lo": "2dsphere" })
    db.geodata.insert( [
    {
        "_id": obj1Id,
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
 ] )
};
addData();