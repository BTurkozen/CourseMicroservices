FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
COPY Services/Discount/Course.Services.Discount/*.csproj Services/Discount/Course.Services.Discount/
RUN dotnet restore Services/Discount/Course.Services.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/Course.Services.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Services.Discount.dll"]