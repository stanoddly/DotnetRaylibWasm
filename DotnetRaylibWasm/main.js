// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

setModuleImports('main.js', {
    window: {
        location: {
            href: () => globalThis.window.location.href
        }
    }
});

const exports = await getAssemblyExports(getConfig().mainAssemblyName);
await dotnet.run();

// TODO: is there a better way to setup canvas directly in dotnet.run or prior that? 
dotnet.instance.Module['canvas'] = (function() { return document.getElementById('canvas'); })();

// this calls our C# code using raylib 
exports.DotnetRaylibWasm.RayTest.Start();
