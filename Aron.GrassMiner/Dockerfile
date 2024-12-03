ARG TARGETARCH
ARG PROJECT_NAME=Aron.GrassMiner
ARG OS=linux

FROM --platform=$OS/$TARGETARCH mcr.microsoft.com/dotnet/sdk:8.0.404-bookworm-slim AS build
ARG TARGETARCH
ARG PROJECT_NAME
ARG OS


ARG BUILD_CONFIGURATION=Release
ENV TARGETARCH=$TARGETARCH
ENV PROJECT_NAME=$PROJECT_NAME

WORKDIR /src

RUN uname -m


RUN apt-get update && apt-get install -y \
    ca-certificates \
    curl \
    clang \
    gcc \
    libc6-dev \
    libunwind-dev \
    zlib1g-dev \
    gnupg \
     && rm -rf /var/lib/apt/lists/*


COPY nuget.config ./ 
COPY $PROJECT_NAME.csproj ./
RUN dotnet restore "$PROJECT_NAME.csproj"

COPY . ./

#RUN case "${TARGETARCH}" in \
        #amd64) RID="x64" ;; \
        #arm64) RID="arm64" ;; \
        #arm/*) RID="arm" ;; \
        #386)   RID="x86" ;; \
        #*)     echo "Unsupported architecture: ${TARGETARCH}"; exit 1 ;; \
    #esac \
    #dotnet publish "$PROJECT_NAME.csproj" -c $BUILD_CONFIGURATION -o /app/publish --self-contained -r $OS-$RID /p:PublishAot=true
#

RUN case "$TARGETARCH" in \
        amd64)  RID="x64" ;; \
        arm64)  RID="arm64" ;; \
        arm/*)  RID="arm" ;; \
        386)   RID="x86" ;; \
        *)     echo "Unsupported architecture: $TARGETARCH"; exit 1 ;; \
    esac && \
    dotnet publish "$PROJECT_NAME.csproj" -c $BUILD_CONFIGURATION \
        -o /app/publish \
        --self-contained \
        -r $OS-$RID \
        /p:PublishAot=true


FROM --platform=$OS/$TARGETARCH debian:bookworm-20241111-slim AS final
ARG TARGETARCH
ARG PROJECT_NAME
ENV PROJECT_NAME=$PROJECT_NAME

EXPOSE 8080/tcp
ENV ASPNETCORE_HTTP_PORTS=8080


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

RUN chmod +x ${PROJECT_NAME}

ENTRYPOINT ["./$PROJECT_NAME"]
