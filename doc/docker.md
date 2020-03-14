### Deploy for production by using a compose file
    docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

### Exec a bash session (for watching log or upload files)
    docker exec -it <container name> bash

### Remove an image
    docker rmi <image>

### Remove the images after building
    docker rmi $(docker images -f “dangling=true” -q)

### Export image
    docker save -o file.tar <image>

###  Import image
    docker load --input file.tar