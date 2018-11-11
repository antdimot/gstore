# GSTore
### Abstract
There are a lot of contents that's need to be linked with a particular location. This service provides a set of rest api for storing contents using latitude and longitude, which makes easier to find them later.

### Examples
Request for getting a valid access token by username and password.

    curl -X POST --data "username=admin&password=P@ssw0rd" http://localhost:5000/api/v1.0/user/authenticate


Request for getting contents by latitude, longitude and distance (Km).

    curl -X GET -H "Accept: application/json" -H "Authorization: Bearer ACCESS-TOKEN-HERE" "http://localhost:5000/api/v1.0/geodata/location?lon=-74.046689&lat=40.68924941&distance=0.1"


## Building and running instructions
1. git clone https://github.com/antdimot/gstore.git
2. cd gstore/src/app
3. docker-compose up -d
4. check api doc at http://localhost:5000/api/swagger

Another way for testing GSTore is by using PWD, which is a simple, interactive and playground docker environment.

[![Try in PWD](https://raw.githubusercontent.com/play-with-docker/stacks/master/assets/images/button.png)](https://labs.play-with-docker.com/?stack=https://raw.githubusercontent.com/antdimot/gstore/master/stack.yml)


## Useful information
The service uses [JWT](https://en.wikipedia.org/wiki/JSON_Web_Token) for securely transmitting information between parties, so that refer to appsettings.*.json files for update token_appkey with your.
The gstore database is initialized by docker with contents of file [data.js](https://raw.githubusercontent.com/antdimot/gstore/master/src/app/db/data.js), which contains information for access and some contents.
