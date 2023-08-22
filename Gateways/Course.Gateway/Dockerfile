FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY Gateways/Course.Gateway/*.csproj Gateways/Course.Gateway/
RUN dotnet restore Gateways/Course.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/Course.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Gateway.dll"]