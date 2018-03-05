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
        "lname": "Di Motta"
    } ] )
};
addUsers();

var obj1Id = ObjectId();
var addObjects = ()=> {
    db.geoobject.drop();
    db.geoobject.insert( [
    {
        "_id": obj1Id,
        "lat": "1",
        "lon": "1",
        "data": "Hello World!",
        "name": "Example",
        "uid" : user1Id
    } ] )
};
addObjects();