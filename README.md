# DotnetRaylibWasm

This is a work-in-progress prototype for .NET 7 with Raylib compiled into WebAssembly (wasm). It's purpose is to demonstrate the capability once it's finished. Check the list of [known issues](https://github.com/disketteman/DotnetRaylibWasm/issues).

## Setup

Make sure you have the this or latest version of .NET 7 (for example [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)):
```
dotnet --version
7.0.100
```

Install the official wasm tooling:

```
dotnet workload install wasm-tools
dotnet workload install wasm-experimental
```

Install a tool to create ad hoc http server to server `application/wasm`:

```
dotnet tool install --global dotnet-serve
```

`libraylib.a` is included in the repository from `raylib-4.2.0_webassembly.zip` from this [source](https://github.com/raysan5/raylib/releases/tag/4.2.0).

## Run it

Build the solution either from your IDE or from command line within the root of the repository:

```
dotnet build
```

To server the files use this command from the root (adjust `\` to `/` if you are not on Windows):

```
dotnet serve --mime .wasm=application/wasm --mime .js=text/javascript --mime .json=application/json --directory DotnetRaylibWasm\bin\Debug\net7.0\browser-wasm\AppBundle\
```

Copy the link from the command and open it in your browser.

Don't use something else like for example serving via Python! The one above will make sure that `.wasm` files are served with `application/wasm`, so it's recognized by browsers and really handled as WebAssembly.

> If mimeType is not `application/wasm`, reject returnValue with a `TypeError` and abort these substeps.\
> ([source](https://webassembly.org/docs/web/#process-a-potential-webassembly-response))

## Dead ends

* referencing Raylib-Cs doesn't work due to bug in dotnet wasm, it create some wrapper in C++ but the `-` is interpreted as invalid symbol during compilation and the 
* For whatever reason with `[DllImport]` couldn't find the library, but with .NET 7 `[LibraryImport]` it can
* .NET 7 before the final release didn't work, probably a problem of an older version of emscripten, see [here](https://www.reddit.com/r/Blazor/comments/x1rqgx/comment/imfe71r/?context=3)

## References

* https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies
* https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/
* https://devblogs.microsoft.com/dotnet/use-net-7-from-any-javascript-app-in-net-7/
* `[LibraryImport]` https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke-source-generation
  * Compatibility differences between `[LibraryImport]` and `[DllImport]` https://github.com/dotnet/runtime/blob/main/docs/design/libraries/LibraryImportGenerator/Compatibility.md
