# GSTore

There are a lot of contents that's need to be linked with a particular location. This service provides a set of rest api for storing contents using latitude and longitude, which makes easier to find them later.

## Examples

Making request for getting a valid access token by username and password.

    curl -X POST --data "username=admin&password=P@ssw0rd" http://localhost:5000/api/v1.0/user/authenticate


Making request for getting contents by latitude, longitude and distance (km).

    curl -X GET -H "Accept: application/json" -H "Authorization: Bearer ACCESS-TOKEN-HERE" "http://localhost:5000/api/v1.0/geodata/location?lon=-74.046689&lat=40.68924941&distance=0.1"


## Instructions for building and running by Docker

1. git clone https://github.com/antdimot/gstore.git
2. cd gstore/src/app
3. docker-compose up -d
4. open your browser to http://localhost:5000/api/swagger

## Play with Docker
Another way for testing GSTore is by using PWD which is a simple, interactive and playground environment.

[![Try in PWD](https://raw.githubusercontent.com/play-with-docker/stacks/master/assets/images/button.png)](https://labs.play-with-docker.com/?stack=https://raw.githubusercontent.com/antdimot/gstore/master/stack.yml)

