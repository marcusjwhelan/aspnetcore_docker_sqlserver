 # Project Init
 https://www.youtube.com/watch?v=fmvcAzHpsk8
 
 Needed for toolset
 ```bash
dotnet tool install --global dotnet-ef
 ```
 
 ```bash
dotnet new webapi -n Commander
 ```

Created some mock data
```bash
dotnet run
```
API Endpoints to hit on postman
http://localhost:5000/api/command/
http://localhost:5000/api/command/0

Add Packages
```bash
dotnet add package Microsoft.EntityFrameworkCore --version 3.1.9
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.9
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.9
dotnet add package MySql.Data.EntityFrameworkCore --version 8.0.22
dotnet add package MySql.Data.EntityFrameworkCore.Design --version 8.0.19
```

After setting up the SQL configuration in startup
run the migrations - InitialMigration is the name. Creates
a new folder called migrations.

Did not have to create any database things, this just creates
the migrations folder with up and down options
```bash
dotnet ef migrations add InitialMigration
```
This creates a migration but made some fields nullable so change
it with, removes migrations that are not ran.
```bash
dotnet ef migrations remove
```
To Run the migrations
```bash
dotnet ef database update
```

Packages to do auto mapping with DTOs
```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 8.1.0
```
Attach service in startup class.


# Docker setup
#### Setting up Asp.Net Core with Docker
Create the Dockerfile and the .dockerignore file

##### Setup https in docker for local host on WINDOWS
In my case I was using powershell.

Should be in equivalent C:\Users\marcus.whelan\.aspnet\https\Commander.pfx

Export the certificate and password not in powershell, password doesn't have to match anything.
```bash
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Commander.pfx -p pa55w0rd!
dotnet dev-certs https --trust
```
in powershell +
```bash
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\Commander.pfx -p { password here }
dotnet dev-certs https --trust
```
on mac or linux
```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/Commander.pfx -p { password here }
dotnet dev-certs https --trust
```

Store the password with windows user secrets for local dev.

First in Commander.csproj add `<UserSecretsId></UserSecretsId>` in `<UserPropertyGroup>`.
And go to https://guidgenerator.com/online-guid-generator.aspx and put in a random guid there

then run as , set "key" "value" where the key is a special value that does this quickly
```bash
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "pa55w0rd!"
> Successfully saved Kestrel:Certificates:Development:Password = pa55w0rd! to the secret store.
```
Which then stores it at C:\Users\marcus.whelan\AppData\Roaming\Microsoft\UserSecrets\59143296-664c-4c36-a5e5-ef2c4073936e\secrets.json

where secrets.json looks like
```json
{
  "Kestrel:Certificates:Development:Password": "pa55w0rd!"
}
```

#### Build the image

Build the image and tag the image
```bash
docker build -t mjwrazor/commanderapi:latest .
```
To run, port 80 was exposed. But we want to expose a different port on our machine
```bash
docker run -p 5000:80 mjwrazor/commanderapi:latest 
```
and see it on http://localhost:5000/api/commands

Take image and deploy to docker hub
```bash
docker push mjwrazor/commanderapi:latest
```
Deploying docker image on azure. Go to azure portal.

* Create Resource -> Container instances -> click create
* Create new Resource group
* name -> commanderapicontainer
* region -> choose a region
* type -> public
* image name -> mjwrazor/commanderapi
* size -> pick one

Move on to networking. left as default since port 80 was used. provide dns name
commanderapicontainer = url.

No changes in advanced,tags,in review and create hit "Create".
FQN thingy is the domain name you can visit.

#### Getting SQL server running
```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Pa55w0rd2019' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
```
You will need to install the SQL server tools in the container
start shell with these command tools running
```bash
docker exec -it {containerId} /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'Pa$$w0rd2019'
1> select name from sys.sysdatabases;
2> go;
```
Puts you right into a place to put in commands 
 
#### Docker Compose
This should be the go to for local development

Get the project going, making sure to have set up local https for .net core
```bash
docker-compse up --build
```
