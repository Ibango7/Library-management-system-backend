FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/LmsBackend.Web.Host/LmsBackend.Web.Host.csproj", "src/LmsBackend.Web.Host/"]
COPY ["src/LmsBackend.Web.Core/LmsBackend.Web.Core.csproj", "src/LmsBackend.Web.Core/"]
COPY ["src/LmsBackend.Application/LmsBackend.Application.csproj", "src/LmsBackend.Application/"]
COPY ["src/LmsBackend.Core/LmsBackend.Core.csproj", "src/LmsBackend.Core/"]
COPY ["src/LmsBackend.EntityFrameworkCore/LmsBackend.EntityFrameworkCore.csproj", "src/LmsBackend.EntityFrameworkCore/"]
WORKDIR "/src/src/LmsBackend.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/LmsBackend.Web.Host", "src/LmsBackend.Web.Host"]
COPY ["src/LmsBackend.Web.Core", "src/LmsBackend.Web.Core"]
COPY ["src/LmsBackend.Application", "src/LmsBackend.Application"]
COPY ["src/LmsBackend.Core", "src/LmsBackend.Core"]
COPY ["src/LmsBackend.EntityFrameworkCore", "src/LmsBackend.EntityFrameworkCore"]
WORKDIR "/src/src/LmsBackend.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "LmsBackend.Web.Host.dll"]
