#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
USER root
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Aron.GrassMiner.csproj .
RUN dotnet restore "Aron.GrassMiner.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Aron.GrassMiner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Aron.GrassMiner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt update && apt-get install -y wget
RUN wget -q https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
RUN apt-get install -y ./google-chrome-stable_current_amd64.deb
RUN mkdir /vpnclient
RUN apt-get install -y unzip

COPY --from=publish /app/publish .
RUN unzip /app/vpnclient.zip -d /
RUN rm /app/vpnclient.zip
WORKDIR /vpnclient
RUN apt-get update && apt-get install -y gcc make 
RUN sh .install.sh
RUN apt-get install -y isc-dhcp-client nano curl net-tools iputils-ping


WORKDIR /app
#CMD ./run.sh
ENTRYPOINT ["dotnet", "Aron.GrassMiner.dll"]
