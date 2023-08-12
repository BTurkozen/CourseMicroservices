FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
EXPOSE 80
COPY Gateways/Course.Gateway/*.csproj Gateways/Course.Gateway/
RUN dotnet restore Gateways/Course.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/Course.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Gateway.dll"]