# Get base SDK Image from Microsoft to build the project in
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /app

# Copy the csproj file and restore any dependencies (via nuget)
COPY *.csproj ./
RUN dotnet restore

# Copy the project files and build our release
COPY . ./
COPY ./wait-for-it.sh /wait-for-it.sh
RUN dotnet publish -c Release -o out

# Generate runtime image, the image the container will be running on
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
COPY --from=build-env /app/wait-for-it.sh .
RUN chmod +x wait-for-it.sh
RUN apt-get update -y &&\
 apt-get install -y dos2unix
RUN dos2unix wait-for-it.sh
ENTRYPOINT ["dotnet","Commander.dll"]