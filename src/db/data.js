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
var obj2Id = ObjectId();
var obj3Id = ObjectId();
var obj4Id = ObjectId();
var addData = ()=> {
    db.geodata.drop();
    db.geodata.insert( [
    {
        "_id": obj1Id,
        "lat": "40.848082",
        "lon": "14.283069",
        "content": "Mate naples site 2",
        "name": "Mate2",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj2Id,
        "lat": "40.850524",
        "lon": "14.274842",
        "content": "Hotel Ramada",
        "name": "Hotel",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj3Id,
        "lat": "40.852815",
        "lon": "14.271879",
        "content": "Naples train station",
        "name": "Station",
        "ctype": "text/plain",
        "uid" : user1Id
    },
    {
        "_id": obj4Id,
        "lat": "40.681562",
        "lon": "14.774473",
        "content": "Mate naples site 1",
        "name": "Mate1",
        "ctype": "text/plain",
        "uid" : user1Id
    },
 ] )
};
addData();