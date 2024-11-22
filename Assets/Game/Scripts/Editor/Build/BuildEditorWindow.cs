﻿using System.IO;
using Game.Editor;
using GameFramework;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor.ResourceTools;
using UnityGameFramework.Extension.Editor;

namespace Game.Scripts.Editor.Build
{
    public class BuildEditorWindow : EditorWindow
    {
        private Platform m_Platform;

        [MenuItem("Tools/Build Tool Editor")]
        public static void ShowWindow()
        {
            GetWindow<BuildEditorWindow>("Build Tool");
        }

        public static Platform GetCurPlatform()
        {
#if UNITY_IOS
            return Platform.IOS;
#elif UNITY_ANDROID
            return Platform.Android;
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return Platform.Windows64;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            return Platform.MacOS;
#endif
        }

        private void OnEnable()
        {
            m_Platform = GetCurPlatform();
        }

        private void OnGUI()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            {
                m_Platform = (Platform)EditorGUILayout.EnumPopup("Platform: ", m_Platform);

                if (GUILayout.Button("Build Pkg (Contains Rebuild Resource)"))
                {
                    if (SwitchActiveBuildTarget(m_Platform))
                    {
                        BuildHelper.BuildPkg(m_Platform);
                        Debug.Log($"Build {m_Platform} Pkg Success!");
                    }
                }

                if (GUILayout.Button("Build Resource"))
                {
                    if (SwitchActiveBuildTarget(m_Platform))
                    {
                        BuildHelper.BuildResource(m_Platform);
                        Debug.Log($"Build {m_Platform} Resource Success!");
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private bool SwitchActiveBuildTarget(Platform platform)
        {
            if (platform != GetCurPlatform())
            {
                switch (EditorUtility.DisplayDialogComplex("Warning!",
                            $"current platform is {GetCurPlatform()}, if change to {platform}, may be take a long time",
                            "change", "cancel", null))
                {
                    case 0:
                        break;
                    default:
                        return false;
                }
            }

            BuildTarget buildTarget = BuildTarget.NoTarget;
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Unknown;
            switch (platform)
            {
                case Platform.Windows:
                    buildTarget = BuildTarget.StandaloneWindows;
                    buildTargetGroup = BuildTargetGroup.Standalone;
                    break;
                case Platform.Windows64:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    buildTargetGroup = BuildTargetGroup.Standalone;
                    break;
                case Platform.Android:
                    buildTarget = BuildTarget.Android;
                    buildTargetGroup = BuildTargetGroup.Android;
                    break;
                case Platform.IOS:
                    buildTarget = BuildTarget.iOS;
                    buildTargetGroup = BuildTargetGroup.iOS;
                    break;
                case Platform.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    buildTargetGroup = BuildTargetGroup.Standalone;
                    break;
                case Platform.Linux:
                    buildTarget = BuildTarget.StandaloneLinux64;
                    buildTargetGroup = BuildTargetGroup.Standalone;
                    break;
                case Platform.WebGL:
                    buildTarget = BuildTarget.WebGL;
                    buildTargetGroup = BuildTargetGroup.WebGL;
                    break;
                default:
                    throw new GameFrameworkException($"No Support {platform}!");
            }

            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            return true;
        }
        
    }
}