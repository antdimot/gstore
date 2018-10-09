# README

## Instructions for building the solution

#### START APPLICATION:
    1. cd src/app
    2. docker-compose up -d


#### DOCKER INFORMATION:
    docker rmi $(docker images -f “dangling=true” -q)
