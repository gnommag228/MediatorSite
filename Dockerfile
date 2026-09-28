FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY *.csproj ./
COPY MediatorSite.Tests/*.csproj ./MediatorSite.Tests/
RUN dotnet restore

COPY . ./


RUN dotnet test "MediatorSite.Tests/MediatorSite.Tests.csproj" -c Release

RUN dotnet publish "MediatorSite.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MediatorSite.dll"]