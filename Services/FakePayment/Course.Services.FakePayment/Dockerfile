FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
COPY Shared/Course.Shared/*.csproj Shared/Course.Shared/
COPY Services/FakePayment/Course.Services.FakePayment/*.csproj Services/FakePayment/Course.Services.FakePayment/
RUN dotnet restore Services/FakePayment/Course.Services.FakePayment/*.csproj
COPY . .
RUN dotnet publish Services/FakePayment/Course.Services.FakePayment/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Course.Services.FakePayment.dll"]