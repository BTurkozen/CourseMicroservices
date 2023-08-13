FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
EXPOSE 80
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
COPY Services/Basket/Course.Services.Basket/*.csproj Services/Basket/Course.Services.Basket/
RUN dotnet restore Services/Basket/Course.Services.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/Course.Services.Basket/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Services.Basket.dll"]