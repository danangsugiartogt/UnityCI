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
        PlayerSettings.Android.keystoreName = GetArg("-androidKeystoreName");
        PlayerSettings.Android.keystorePass = GetArg("-androidKeystorePass");
        PlayerSettings.Android.keyaliasName = GetArg("-androidKeyaliasName");
        PlayerSettings.Android.keyaliasPass = GetArg("-androidKeyaliasPass");

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
        foreach (var arg in args)
        {
            if (arg.Contains(name))
            {
                var split = arg.Split('=');
                if (split.Length == 2)
                    return split[1];
            }
        }

        return null;
    }
}