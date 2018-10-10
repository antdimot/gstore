# GSTORE
A service for storing and retrieving data by latitude and longitude.

## Instructions for building the solution

#### START APPLICATION:
    1. cd src/app
    2. docker-compose up -d
    3. curl -X POST -d "username=antonio.dimotta@gmail.com&password=P@ssw0rd" http://localhost:5000/api/v1.0/user/authenticate
    4. curl -H "Accept: application/json" -H "Authorization: Bearer ${TOKEN}" http://localhost:5000/api/v1.0/geodata/location?lon=14.2749&lat=40.850506&distance=0.5


TOKEN=$(curl -s -X POST -H 'Accept: application/json' -H 'Content-Type: application/json' --data '{"username":"antonio.dimotta@gmail.com","password":"P@ssw0rd"' http://localhost:5000/api/v1.0/user/authenticate | jq -r '.token')

curl -H 'Accept: application/json' -H "Authorization: Bearer ${TOKEN}" ttp://ocalhost:5000/api/v1.0/geodata/location?lon=14.2749&lat=40.850506&distance=0.5