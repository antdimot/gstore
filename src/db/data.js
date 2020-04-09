// Example of database initialization

/*
Add application users enabled to access.
The property "au" set the api authorization policies
*/
var db = db.getSiblingDB("gstore");
// hashed value of string P@ssw0rd
var password = "HY0Xg3z9/kqaY/xf+YU5FA==";

var adminId = ObjectId();
var userId = ObjectId();

var addUsers = ()=> {
    db.user.drop();
    db.user.createIndex({'uname':1});
    db.user.insert( [
        {
            "_id": adminId,
            "un": "admin",
            "pw": password,
            "fn": "firstname_admin",
            "ln": "lastname_admin",
            "en": true,
            "au": ["admin","reader","writer"]
        }
    ] );

    db.user.insert( [
        {
            "_id": userId,
            "un": "user1",
            "pw": password,
            "fn": "firstname_user1",
            "ln": "lastname_user1",
            "en": true,
            "au": ["reader","writer"]
        }
    ] );
};
addUsers();

/*
Add localized contents information.
*/
var addData = ()=> {
    db.geodata.drop();
    db.geodata.createIndex({ "lo": "2dsphere" })
    db.geodata.insert( [
    {
        "_id": ObjectId(),
        "lo": {
            "type": "Point",
            "coordinates": [-74.046689,40.68924941]
         },
        "cn": "Statue of Liberty",
        "nm": "Statue of Liberty",
        "ct": "text/plain",
        "tg": ["monument"],
        "ui" : userId
    },
    {
        "_id": ObjectId(),
        "lo": {
            "type": "Point",
            "coordinates": [14.772207,40.675149]
         },
        "cn": "Statue of Liberty",
        "nm": "Salerno station",
        "ct": "text/plain",
        "tg": ["station"],
        "ui" : userId
    },
 ] )
};
addData();