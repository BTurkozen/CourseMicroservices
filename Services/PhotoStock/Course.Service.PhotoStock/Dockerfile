FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
EXPOSE 80
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
COPY Services/PhotoStock/Course.Service.PhotoStock/*.csproj Services/PhotoStock/Course.Service.PhotoStock/
RUN dotnet restore Services/PhotoStock/Course.Service.PhotoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/Course.Service.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Service.PhotoStock.dll"]