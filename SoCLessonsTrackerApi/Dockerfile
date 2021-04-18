#pull official base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

#copy the csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

#copy project files and build the release
COPY . ./
RUN dotnet publish -c Release -o out

#generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "server.dll"]