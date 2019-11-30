FROM mcr.microsoft.com/dotnet/core/sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "irudd-todo.dll"]
#Build: docker build -t iruddtodo .
#Run locally: docker run --name iruddtodo_app --rm -it -p 8000:80 iruddtodo