﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PadelApp/PadelApp.csproj", "PadelApp/"]
COPY ["PadelApp.Application/PadelApp.Application.csproj", "PadelApp.Application/"]
COPY ["PadelApp.Domain/PadelApp.Domain.csproj", "PadelApp.Domain/"]
COPY ["PadelApp.Infrastructure/PadelApp.Infrastructure.csproj", "PadelApp.Infrastructure/"]
COPY ["PadelApp.Persistance/PadelApp.Persistance.csproj", "PadelApp.Persistance/"]
COPY ["PadelApp.Presentation/PadelApp.Presentation.csproj", "PadelApp.Presentation/"]
RUN dotnet restore "PadelApp/PadelApp.csproj"
COPY . .
WORKDIR "/src/PadelApp"
RUN dotnet build "PadelApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PadelApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PadelApp.dll"]
