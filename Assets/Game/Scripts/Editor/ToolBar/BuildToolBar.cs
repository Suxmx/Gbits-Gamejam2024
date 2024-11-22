using Game.Scripts.Editor.Build;
using ToolbarExtension;
using UnityEngine;

namespace Game.Scripts.Editor
{
    sealed class BuildToolBar
    {
        private static readonly GUIContent s_BuildButtonGUIContent =
            new GUIContent("Build", "Open Build Editor!");
        [Toolbar(OnGUISide.Right, 2)]
        private static void OnToolbarGUI()
        {
            if (GUILayout.Button(s_BuildButtonGUIContent))
            {
                BuildEditorWindow.ShowWindow();
            }
        }
    }
}