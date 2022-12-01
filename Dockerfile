FROM mcr.microsoft.com/dotnet/sdk:7.0

RUN dotnet workload install wasm-experimental wasm-tools

RUN apt update
RUN apt install --fix-missing -y \
    cmake \
    build-essential \
    libasound2-dev \
    libgl1-mesa-dev \
    libglu1-mesa-dev \
    libx11-dev \
    libxi-dev \
    libxrandr-dev \
    mesa-common-dev \
    ninja-build \
    xorg-dev \
    wget \
    unzip \
    python3 \
    zsh

RUN git clone https://github.com/emscripten-core/emsdk.git /emsdk
RUN /emsdk/emsdk install 3.1.12
RUN /emsdk/emsdk activate 3.1.12

env SHELL=/bin/zsh

