var db = db.getSiblingDB("demo");
var password = "P@ssw0rd";

var user1Id = ObjectId();
var addUsers = ()=> {
    db.user.drop();
    db.user.createIndex({'uname':1});
    db.user.insert( [
    {
        "_id": user1Id,
        "uname": "adimo",
        "pword": password,
        "email": "antonio.dimotta@gmail.com",
        "fname": "Antonio",
        "lname": "Di Motta",
        "enabled": "true"
    } ] )
};
addUsers();

var obj1Id = ObjectId();
var obj2Id = ObjectId();
var obj3Id = ObjectId();
var obj4Id = ObjectId();
var addData = ()=> {
    db.geodata.drop();
    db.geodata.createIndex({ location: "2dsphere" })
    db.geodata.insert( [
    {
        "_id": obj1Id,
        "location": {
            "type": "Point",
            "coordinates": [14.283069,40.848082]
         },
        "content": "Mate naples site 2",
        "name": "Mate2",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj2Id,
        "location": {
            "type": "Point",
            "coordinates": [14.274900,40.850506]
         },
        "content": "Hotel Ramada",
        "name": "Hotel",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj3Id,
        "location": {
            "type": "Point",
            "coordinates": [14.271879,40.852815]
         },
        "content": "Naples train station",
        "name": "Station",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj4Id,
        "location": {
            "type": "Point",
            "coordinates": [14.774473,40.681562]
         },
        "content": "Mate naples site 1",
        "name": "Mate1",
        "ctype": "text/plain",
        "uid" : user1Id
    },
 ] )
};
addData();