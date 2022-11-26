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

//var Module = globalThis.Module;

//document.getElementById('out').innerHTML = text;
const exports = await getAssemblyExports(getConfig().mainAssemblyName);
await dotnet.run();

console.log("What's the module?");

dotnet.instance.Module['canvas'] = (function() { return document.getElementById('canvas'); })();
console.log(dotnet.instance.Module.canvas);

await exports.WebTest.RayThing.Start();
