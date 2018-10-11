# GSTORE
A service for storing and retrieving data by latitude and longitude.

## Instructions for building and running solution

#### START APPLICATION:
    1. git clone https://github.com/antdimot/gstore.git
    1. cd gstore/src/app
    2. docker-compose up -d
    3. curl -X POST --data "username=antdimot&password=P@ssw0rd" http://localhost:5000/api/v1.0/user/authenticate
    4. curl -X GET -H "Accept: application/json" -H "Authorization: Bearer [TOKEN]" "http://localhost:5000/api/v1.0/geodata/location?lon=14.2749&lat=40.850506&distance=0.5"