var db = db.getSiblingDB("demo");

db.user.drop();

db.user.insert({"firstname":"Antonio","lastname":"Di Motta"});