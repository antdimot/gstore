// sample database initialization data

/*
add application users enabled to access.
The property "au" set the api authorization policies
*/
var db = db.getSiblingDB("gstore");

// set an hashed password which orginal value is P@ssw0rd
var samplePassword = "HY0Xg3z9/kqaY/xf+YU5FA==";

var adminId = ObjectId();
var userId = ObjectId();

var addUsers = ()=> {
    db.user.drop();
    db.user.createIndex({'uname':1});
    db.user.insert( [
        {
            "_id": adminId,
            "un": "admin",
            "pw": samplePassword,
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
            "pw": samplePassword,
            "fn": "firstname_user1",
            "ln": "lastname_user1",
            "en": true,
            "au": ["reader","writer"]
        }
    ] );
};
addUsers();

// add localized contents information
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
        "cn": "Salerno",
        "nm": "Salerno",
        "ct": "text/plain",
        "tg": ["city"],
        "ui" : userId
    },
    {
        "_id": ObjectId(),
        "lo": {
            "type": "Point",
            "coordinates": [14.268120,40.851799]
         },
        "cn": "Naples",
        "nm": "Naples",
        "ct": "text/plain",
        "tg": ["city"],
        "ui" : userId
    },
 ] )
};
addData();