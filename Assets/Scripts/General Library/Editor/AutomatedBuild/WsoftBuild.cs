/*
 * Programmatically build the game for many platforms
 * @Jacob Shreve
 * Thanks to @Austin Yarger and Arbor Interactive
 */

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace WSoft.Tools.AutomatedBuild
{
    public class WsoftBuild
    {

#if UNTIY_EDITOR_WIN
    public static string BUILD_DIRECTORY = @"C:\temp\WsoftBuild\";
#else
        public static string BUILD_DIRECTORY = @"/tmp/WsoftBuild/";
#endif

        public static string PROJECT_NAME = "ProjectBlue";

        [MenuItem("WolverineSoft/Build/All")]
        public static void BuildAll()
        {
            MacOS();
            Win();
            Linux();
        }

        [MenuItem("WolverineSoft/Build/MacOS")]
        public static bool MacOS()
        {
            return ReportBuild("MacOS",
                BuildPipeline.BuildPlayer(
                    GetScenePaths(),
                    BUILD_DIRECTORY + PROJECT_NAME + ".app",
                    BuildTarget.StandaloneOSX,
                    BuildOptions.None
                )
            );
        }

        [MenuItem("WolverineSoft/Build/Windows")]
        public static bool Win()
        {
            string path = BUILD_DIRECTORY + "windows/";

            return ReportBuild("Windows",
                BuildPipeline.BuildPlayer(
                    GetScenePaths(),
                    path + PROJECT_NAME + ".exe",
                    BuildTarget.StandaloneWindows,
                    BuildOptions.None
                )
            );
        }

        [MenuItem("WolverineSoft/Build/Linux")]
        public static bool Linux()
        {
            return ReportBuild("Linux",
                BuildPipeline.BuildPlayer(
                    GetScenePaths(),
                    BUILD_DIRECTORY + PROJECT_NAME + ".bin",
                    BuildTarget.StandaloneLinux64,
                    BuildOptions.None
                )
            );
        }

        private static bool ReportBuild(string name, BuildReport report)
        {
            bool success = (report.summary.result == BuildResult.Succeeded);
            if (success)
            {
                Debug.Log(name + " Build Succeeded");
            }
            else
            {
                Debug.Log(name + " Build Failed");
            }
            return success;
        }

        private static string[] GetScenePaths()
        {
            var scene_settings = EditorBuildSettings.scenes;
            string[] scene_paths = new string[scene_settings.Length];

            for (int i = 0; i < scene_paths.Length; i++)
            {
                scene_paths[i] = scene_settings[i].path;
            }

            return scene_paths;
        }
    }
}
#endif