### Deploy with this production Compose file you can run
    docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

### Remove an image
    docker rmi <image>

### Get bash session (for watching log or upload files)
    docker exec -it <container name> bash

### Remove <none> images after building
    docker rmi $(docker images -f “dangling=true” -q)

### Export image
    docker save -o file.tar <image>

###  Import image
    docker load --input file.tar