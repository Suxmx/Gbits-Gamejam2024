using System;
using System.IO;
using System.Reflection;
using Game.Editor;
using GameFramework;
using GameFramework.Resource;
using GameMain.Editor.ResourceTools;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Editor.ResourceTools;
using UnityGameFramework.Extension.Editor;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Editor.Build
{
    public static class BuildHelper
    {
        private static Platform m_OriginalPlatform;
        public static readonly string BuildPkgFolder = Path.GetFullPath("./Build/Pkg");

        public static void BuildResource(Platform platform)
        {
            Debug.Log("start refresh resource collection");
            ResourceRuleEditorUtility.RefreshResourceCollection();
            Debug.Log("finish refresh resource collection");

            Debug.Log("start build resource");
            ResourceBuildHelper.StartBuild(platform);
            Debug.Log("finish build resource");
            CopyToEditorStreamingAssets(platform);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        public static void BuildPkg(Platform platform)
        {
            BuildTarget buildTarget = BuildTarget.NoTarget;
            string appName = Application.productName;
            switch (platform)
            {
                case Platform.Windows:
                    buildTarget = BuildTarget.StandaloneWindows;
                    appName += ".exe";
                    break;
                case Platform.Windows64:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    appName += ".exe";
                    break;
                case Platform.Android:
                    buildTarget = BuildTarget.Android;
                    appName += ".apk";
                    break;
                case Platform.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case Platform.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                case Platform.Linux:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    break;
                case Platform.WebGL:
                    buildTarget = BuildTarget.WebGL;
                    break;
                default:
                    throw new GameFrameworkException($"No Support {platform}!");
            }

            Debug.Log($"start build {platform}");

            string fold = Utility.Path.GetRegularPath($"{BuildPkgFolder}/{platform}");

            if (Directory.Exists(fold))
            {
                FileTool.CleanDirectory(fold);
            }
            else
            {
                Directory.CreateDirectory(fold);
            }

            BuildResource(platform);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            string[] levels =
            {
                EntryUtility.EntryScenePath,
            };
            Debug.Log("start build pkg");
            string locationPathName = $"{fold}/{appName}";

            PlayerSettings.SetScriptingBackend(
                NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(buildTarget)),
                ScriptingImplementation.Mono2x
            );
            // PlayerSettings.SetIl2CppCodeGeneration(
            //     NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(buildTarget)),
            //     Il2CppCodeGeneration.OptimizeSpeed);

            BuildReport buildReport =
                BuildPipeline.BuildPlayer(levels, locationPathName, buildTarget, BuildOptions.None);
            if (buildReport.summary.result != BuildResult.Succeeded)
            {
                throw new GameFrameworkException($"build pkg fail : {buildReport.summary.result}");
            }

            Debug.Log($"finish build pkg at {locationPathName}");
            EditorUtility.OpenWithDefaultApp(fold);
        }
        public static void CopyToEditorStreamingAssets(Platform platform)
        {
            string targetPath = $"{Application.streamingAssetsPath}";
            if (!Directory.Exists(targetPath))
            {
                throw new GameFrameworkException($"RefreshExePkgResource fail! {targetPath} not exist!");
            }

            FileTool.CleanDirectory(targetPath);
            string bundleFold = Path.Combine(ResourceBuildHelper.GetNewestBundlePath(), platform.ToString());
            bundleFold = Utility.Path.GetRegularPath(bundleFold);
            FileTool.CopyDirectory(bundleFold, targetPath);
            Debug.Log($"src dir: {bundleFold}    target: {targetPath}");
        }

    }
}