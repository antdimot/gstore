
# Using [The mongo Shell](https://docs.mongodb.com/manual/mongo/) with GSTore

### exec bash command into mongodb container
    docker exec -it <mongo-container-id> bash

### exec mongo client
    mongo

### database list
    show dbs

### show current db
    db

### change current database
    use gstore

### show collections
    show collections

### show collection items
    db.user.find()
    db.user.find({"un":"user1"})
