using UnrealBuildTool;
using System;
using System.IO;
using System.Diagnostics;

public class ZeroMQ : ModuleRules
{
    private string ThirdPartyPath => Path.GetFullPath(Path.Combine(ModuleDirectory, "..", "..", "ThirdParty"));

    private string ZeroMQRootPath => Path.GetFullPath(Path.Combine(ThirdPartyPath, "libzmq_4.3.1"));

    public void AddZeroMQ(ReadOnlyTargetRules target)
    {
        // add headers
        PublicIncludePaths.Add(Path.Combine(ZeroMQRootPath, "include"));

        // tell library that it is statically linked
        PublicDefinitions.Add("ZMQ_STATIC");

        var staticLibrary = string.Empty;

        if (target.Platform == UnrealTargetPlatform.Win64) {
            staticLibrary = Path.Combine(ZeroMQRootPath, "Windows", "x64", "libzmq-v141-mt-s-4_3_2.lib");
        } else if (target.Platform == UnrealTargetPlatform.Linux) {
            staticLibrary = Path.Combine(ZeroMQRootPath, "Linux", "libzmq.so");
            PublicAdditionalLibraries.Add("stdc++");
        } else if (target.Platform == UnrealTargetPlatform.Mac) {
            staticLibrary = Path.Combine(ZeroMQRootPath, "MacOS", "libzmq.a");
        } else {
            Console.WriteLine($"Unsupported target platform: {target.Platform}");
            Debug.Assert(false);
        }

        bEnableExceptions = true;

        Console.WriteLine($"Using ZeroMQ static library: {staticLibrary}");
        PublicAdditionalLibraries.Add(staticLibrary);
    }

    public ZeroMQ(ReadOnlyTargetRules target) : base(target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        PublicDependencyModuleNames.AddRange(new[] { "Core" });
        AddZeroMQ(target);
    }
}
