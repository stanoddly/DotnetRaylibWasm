# DotnetRaylibWasm

This is a work-in-progress prototype for .NET 7 with Raylib compiled into WebAssembly (wasm). It's purpose is to demonstrate the capability once it's finished. Check the list of [known issues](https://github.com/disketteman/DotnetRaylibWasm/issues).

## Setup

Make sure you have the latest version of .NET 7 for example from [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0), the prototype was built with:
```
dotnet --version
7.0.100
```

Install the official wasm tooling:

```
dotnet workload install wasm-tools
dotnet workload install wasm-experimental
```

Install a tool to create ad-hoc http server to server `application/wasm`:

```
dotnet tool install --global dotnet-serve
```

`libraylib.a` is included in the repository from `raylib-4.2.0_webassembly.zip` from this [source](https://github.com/raysan5/raylib/releases/tag/4.2.0).

## Overview

Our simple usage of Raylib:
```
./RaylibGame/Program.cs
```

The static web page which is served to the browser:
```
./DotnetRaylibWasm/index.html
```

The Javascript module which setups the dotnet runtime and runs our code using Raylib:
```
./DotnetRaylibWasm/main.js
```

Our own fork of Raylib-cs:
```
./Raylib_cs/
```

## Run it

Build the solution either from your IDE or from command line within the root of the repository:

```
dotnet build
```

To server the files use this command from the root (change `\` to `/` in file path if you are not on Windows):

```
dotnet serve --mime .wasm=application/wasm --mime .js=text/javascript --mime .json=application/json --directory DotnetRaylibWasm\bin\Debug\net7.0\browser-wasm\AppBundle\
```

Copy the link from the output and open it in your browser (tested with the latest Edge, Chrome shuld work too)

Don't use something else like for example serving via Python! The one above will make sure that `.wasm` files are served with `application/wasm`, so it's recognized by browsers and really handled as WebAssembly.

> If mimeType is not `application/wasm`, reject returnValue with a `TypeError` and abort these substeps.\
> ([source](https://webassembly.org/docs/web/#process-a-potential-webassembly-response))

## References

* https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies
* https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/
* https://devblogs.microsoft.com/dotnet/use-net-7-from-any-javascript-app-in-net-7/
* Undocumented properties to tweak the build https://github.com/dotnet/runtime/blob/main/src/mono/wasm/build/WasmApp.targets
* Hidden sample using `NativeFileReference` https://github.com/dotnet/runtime/tree/3b0ef303e0969fa376569baa38a98c870a8f7561/src/mono/sample/wasm/browser-advanced
* Not really public interface of the `dotnet` JS module https://github.com/dotnet/runtime/blob/main/src/mono/wasm/runtime/dotnet.d.ts
* Version of emscripten used by wasm-tools https://github.com/dotnet/runtime/blob/78a625d07982046cd68322b59d0001cda7047dee/src/mono/wasm/emscripten-version.txt
