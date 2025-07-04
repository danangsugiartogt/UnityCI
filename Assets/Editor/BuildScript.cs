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
            scenes = new[] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }
}