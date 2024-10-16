# RMMService Web

Web app with many to many relationship between services and products, with cost calculator service. The uses an embedded relational database(MS SQLite). The DB is seeded during the application start up.

## Installation

1. Make sure you have docker install. https://www.docker.com/
2. Run the below commands to build a docker image.
```bash
docker build -t webapiapp .
```
3. Run the following command to create and deploy the container with the app.
```bash
docker run -d --rm -p 9000:80 --name webapiapp webapiapp
```

## Usage

1. Navigate to the swagger page: http://localhost:9000/swagger/index.html
2. Use the APIs to manipulate the data