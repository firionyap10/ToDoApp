# ToDoApp

To start this project, you are require to install Microsoft SQL Server Management Studio and also install the database in your machine. After that configure the connectionstring, you can run the project.


Docker
-----------------------------------------------
To run as docker need to install docker desktop.

The command to create, debug and run the docker:

Build docker
-------------
docker build -t todoapp:v1.1 -f Dockerfile .

Create docker hosting
---------------------
docker run -d -p 5100:80 todoapp:v1.1

List docker hosting
-------------------
docker ps

Debug docker
--------------
docker logs 42

****This is to remove the docker from the container****
Remove hosting docker
----------
docker rm 54 -f

If you want to run the application as docker. you need to change the connection string point to server="host.docker.internal" or else use server="localhost" for MSSQL.
