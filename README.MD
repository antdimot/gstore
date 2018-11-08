# GSTORE

### Instructions for building and running with Docker

1. git clone https://github.com/antdimot/gstore.git
2. cd gstore/src/app
3. docker-compose up -d
4. access to [api documentation](http://localhost:5000/api/swagger)

### Testing

Making request for getting a valid access token by username and password.

    curl -X POST --data "username=appuser&password=P@ssw0rd" http://localhost:5000/api/v1.0/user/authenticate


Making request for getting contents by latitude, longitude and distance (km).

    curl -X GET -H "Accept: application/json" -H "Authorization: Bearer ACCESS-TOKEN-HERE" "http://localhost:5000/api/v1.0/geodata/location?lon=-74.046689&lat=40.68924941&distance=0.1"
