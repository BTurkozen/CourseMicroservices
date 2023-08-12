FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
EXPOSE 80
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
COPY Frontends/Course.Web/*.csproj Frontends/Course.Web/
RUN dotnet restore Frontends/Course.Web/*.csproj
COPY . .
RUN dotnet publish Frontends/Course.Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Web.dll"]