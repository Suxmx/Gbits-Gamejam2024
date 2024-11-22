using System.Reflection;
using GameFramework.Resource;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace Game.Scripts.Editor.Build
{
    public static class EntryUtility
    {
        public static readonly string EntryScenePath = "Assets/Game/Res/Scenes/Launcher.unity";


        public static Scene GetEntryScene()
        {
            EditorSceneManager.OpenScene(EntryScenePath, OpenSceneMode.Additive);
            return SceneManager.GetSceneByPath(EntryScenePath);
        }

        public static T GetEntrySceneComponent<T>() where T : Component
        {
            Scene entryScene = GetEntryScene();
            foreach (var rootGameObject in entryScene.GetRootGameObjects())
            {
                T component = rootGameObject.GetComponentInChildren<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return null;
        }

        public static ResourceMode GetEntryResourceMode()
        {
            ResourceComponent resourceComponent = GetEntrySceneComponent<ResourceComponent>();
            FieldInfo resourceModeFieldInfo =
                typeof(ResourceComponent).GetField("m_ResourceMode", BindingFlags.Instance | BindingFlags.NonPublic);
            ResourceMode resourceMode = (ResourceMode)resourceModeFieldInfo.GetValue(resourceComponent);
            return resourceMode;
        }
    }
}