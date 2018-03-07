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
var addData = ()=> {
    db.geodata.drop();
    db.geodata.insert( [
    {
        "_id": obj1Id,
        "lat": "1",
        "lon": "1",
        "content": "Hello World!",
        "name": "Example",
        "ctype": "text/plain",
        "uid" : user1Id
    } ] )
};
addData();