FROM debian:bookworm-20241111-slim AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

RUN apt-get update && apt-get install -y \
    wget \
    ca-certificates \
    curl \
    gnupg && \
    wget https://packages.microsoft.com/config/debian/11/prod.list -O /etc/apt/sources.list.d/microsoft-prod.list && \
    wget -q https://packages.microsoft.com/keys/microsoft.asc -O- | apt-key add - && \
    apt-get update && apt-get install -y \
    clang \
    gcc \
    libc6-dev \
    libunwind-dev \
    zlib1g-dev \
    dotnet-sdk-8.0 && \
    rm -rf /var/lib/apt/lists/*

COPY Aron.GrassMiner.csproj ./
COPY ["nuget.config", ""]
RUN dotnet restore "Aron.GrassMiner.csproj"

COPY . ./

WORKDIR "/src"
RUN dotnet publish "Aron.GrassMiner.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained -r linux-x64 /p:PublishAot=true

FROM debian:bookworm-20241111-slim AS final
EXPOSE 50005/tcp
ENV ASPNETCORE_HTTP_PORTS 50005
WORKDIR /app

RUN apt-get update -y
RUN apt-get install -y wget

RUN apt-get install -y --no-install-recommends \
    curl \
    net-tools \
    iputils-ping \
    ca-certificates \
    chromium \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
RUN chmod +x Aron.GrassMiner

ENTRYPOINT ["./Aron.GrassMiner"]
