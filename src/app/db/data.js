// Example of database initialization

/*
Add application users enabled to access.
The property "au" set the api authorization policies
*/
var db = db.getSiblingDB("gstore");
// hashed value of string P@ssw0rd
var password = "HY0Xg3z9/kqaY/xf+YU5FA==";

var user1Id = ObjectId("5aa3a6c360d53c0e5565f3a9");
var addUsers = ()=> {
    db.user.drop();
    db.user.createIndex({'uname':1});
    db.user.insert( [
    {
        "_id": user1Id,
        "un": "admin",
        "pw": password,
        "fn": "firstname",
        "ln": "lastname",
        "en": true,
        "au": ["admin","reader","writer"]
    }
 ] )
};
addUsers();

/*
Add localized contents information.
*/
var obj1Id = ObjectId();
var addData = ()=> {
    db.geodata.drop();
    db.geodata.createIndex({ "lo": "2dsphere" })
    db.geodata.insert( [
    {
        "_id": obj1Id,
        "lo": {
            "type": "Point",
            "coordinates": [-74.046689,40.68924941]
         },
        "cn": "Statue of Liberty",
        "nm": "Station",
        "ct": "text/plain",
        "tg": ["monument"],
        "ui" : user1Id
    },
 ] )
};
addData();