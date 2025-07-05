using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class BuildScript
{
    [MenuItem("CI/Build WebGL CI")]
    public static void BuildWebGLForCI()
    {
        // Set Player Settings
        PlayerSettings.colorSpace = ColorSpace.Gamma;
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm; // Optional

        // Set output path
        string outputPath = "build/WebGL";

        // Build options
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }

    [MenuItem("CI/Build Android CI")]
    public static void BuildAndroidForCI()
    {
        foreach (var arg in System.Environment.GetCommandLineArgs())
            Debug.Log("CLI ARG: " + arg);

        // Setup keystore for signing
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = GetArg("-androidKeystoreName")?.Trim();
        PlayerSettings.Android.keystorePass = GetArg("-androidKeystorePass")?.Trim();
        PlayerSettings.Android.keyaliasName = GetArg("-androidKeyaliasName")?.Trim();
        PlayerSettings.Android.keyaliasPass = GetArg("-androidKeyaliasPass")?.Trim();

        Debug.Log("keystore pass length: " + PlayerSettings.Android.keystorePass.Length);
        Debug.Log("keystore alias length: " + PlayerSettings.Android.keyaliasName.Length);
        Debug.Log("keystore alias pass length: " + PlayerSettings.Android.keyaliasPass.Length);

        // Android min and target version
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;

        // Scripting backend
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

        // Architectures target
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = "build/Android/Game.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == name)
            {
                Debug.Log("GetArg Found: " + args[i]);
                Debug.Log("GetArg Returned: " + args[i + 1]);
                return args[i + 1];
            }
        }

        return null;
    }
}