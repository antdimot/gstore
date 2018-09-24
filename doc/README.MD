# README

## Instructions for building the solution

#### START APPLICATION:
    1. cd src/app
    2. docker-compose up -d


#### DOCKER INFORMATION:
    docker run --rm -v ${PWD}:/src -v gstore-vol01:/data alpine:3.7 cp /src/data.js /data
    docker exec -it gstore-db mongo localhost:27017/gstore /data/db/data.js
    docker rmi $(docker images -f “dangling=true” -q)
    docker cp ../db/data.js hobb-web:/srv/hobb/keys/