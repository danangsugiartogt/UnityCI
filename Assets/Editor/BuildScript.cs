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
        // Setup keystore for signing
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "keystore/user.keystore"; // rel. path
        //PlayerSettings.Android.keystorePass = System.Environment.GetEnvironmentVariable("KEYSTORE_PASS");
        //PlayerSettings.Android.keyaliasName = System.Environment.GetEnvironmentVariable("KEY_ALIAS");
        //PlayerSettings.Android.keyaliasPass = System.Environment.GetEnvironmentVariable("KEY_ALIAS_PASS");
        // hardcoded data for debug
        PlayerSettings.Android.keystorePass = "12345678";
        PlayerSettings.Android.keyaliasName = "unity-ci";
        PlayerSettings.Android.keyaliasPass = "12345678";

        Debug.Log("keystoreName:" + PlayerSettings.Android.keystoreName);
        Debug.Log("keystorePass:" + PlayerSettings.Android.keyaliasPass);
        Debug.Log("keyaliasName:" + PlayerSettings.Android.keyaliasName);
        Debug.Log("keyaliasPass:" + PlayerSettings.Android.keyaliasPass);

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
}