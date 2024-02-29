# DotnetRaylibWasm

<img src="https://raw.githubusercontent.com/disketteman/DotnetRaylibWasm/main/demo.jpg" width="417" height="272">

[GitHub Pages Demo](https://stanoddly.github.io/DotnetRaylibWasm/)

This is a fully working prototype for .NET 8 with Raylib ahead-of-time 🚀 compiled into WebAssembly (wasm). See a couple of [known issues](https://github.com/disketteman/DotnetRaylibWasm/issues) for limitations.

## Local setup

Make sure you have the latest version of .NET 8 for example from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), the prototype was built with:
```
dotnet --version
8.0.200
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

`libraylib.a` included in the repository is pre-compiled from my machine, but GitHub Pages workflow [compiles Raylib form PLATFORM=WEB](https://github.com/disketteman/DotnetRaylibWasm/blob/main/.github/workflows/gh-pages.yml#L33) on its own every build.

## Overview

Examples are copied from [ChrisDill/Raylib-cs-Examples](https://github.com/ChrisDill/Raylib-cs-Examples). Few examples were removed since they used functionality removed from Raylib 4.2. This project uses 4.2.0.2 of the `Raylib-cs` nuget package.

```
./Examples
```

The project which makes the WebAssembly possible is here:
```
./DotnetRaylibWasm
```

## Run it

> **Note**
> The latest version of main branch is available [here](https://disketteman.github.io/DotnetRaylibWasm/). To run it locally follow these instructions.

`publish` the solution from the root of the repository. Don't even try to use just `build`, [it's buggy](https://github.com/disketteman/DotnetRaylibWasm/issues/7#issuecomment-1356442508). Note the publishing takes a while:

```
dotnet publish -c Release
```

To serve the files use this command from the root (change `\` to `/` in file path if you are not on Windows):

```
dotnet serve --mime .wasm=application/wasm --mime .js=text/javascript --mime .json=application/json --directory DotnetRaylibWasm\bin\Debug\net8.0\browser-wasm\AppBundle\
```

Copy the link from the output and open it in your browser (tested on the latest Chrome).

## References

Plenty of information isn't really public or well documented, so this section collects found information:

* https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies
* https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/
* https://devblogs.microsoft.com/dotnet/use-net-7-from-any-javascript-app-in-net-7/
* Undocumented properties to tweak the build https://github.com/dotnet/runtime/blob/main/src/mono/wasm/build/WasmApp.targets
* WasmAppBuilder task https://github.com/dotnet/runtime/tree/ma/src/tasks/WasmAppBuilder
* Hidden sample using `NativeFileReference` https://github.com/dotnet/runtime/tree/3b0ef303e0969fa376569baa38a98c870a8f7561/src/mono/sample/wasm/browser-advanced
* Not really public interface of the `dotnet` JS module https://github.com/dotnet/runtime/blob/main/src/mono/wasm/runtime/dotnet.d.ts
* Version of emscripten used by wasm-tools https://github.com/dotnet/runtime/blob/78a625d07982046cd68322b59d0001cda7047dee/src/mono/wasm/emscripten-version.txt
