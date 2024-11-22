using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor.GFExtension.Editor
{
    public class EventArgsCodeGeneratorWindow : EditorWindow
    {
        private string namespaceName;
        private string className;
        private string savePath;
        private string subdirectory;

        [MenuItem("Tools/GameEventArgs代码生成器")]
        public static void ShowWindow()
        {
            GetWindow<EventArgsCodeGeneratorWindow>("GameEventArgs代码生成器");
        }

        private void OnEnable()
        {
            namespaceName= EditorPrefs.GetString(Application.productName + "_GameEventArgsGenNamespace", "GameMain");
            savePath = EditorPrefs.GetString(Application.productName + "_GameEventArgsGenPath", "Assets");
        }

        private void OnGUI()
        {
            GUILayout.Label("GameEventArgs代码生成器", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            namespaceName = EditorGUILayout.TextField("Namespace", namespaceName);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(Application.productName + "_GameEventArgsGenNamespace", namespaceName);
            }
            className = EditorGUILayout.TextField("Class Name", className);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("路径：", GUILayout.Width(40));
            EditorGUILayout.LabelField(savePath);
            if (GUILayout.Button("选择"))
            {
                ChoosePath();
            }
            EditorGUILayout.EndHorizontal();
            subdirectory = EditorGUILayout.TextField("Subdirectory", subdirectory);
            

            if (GUILayout.Button("Generate Code"))
            {
                GenerateCode();
            }
        }

        private void ChoosePath()
        {
            string defaultPath = $"Assets/";
            string filePath = EditorUtility.OpenFolderPanel("选择GameEventArgs代码存储位置", defaultPath, "");
            if (filePath.Contains("Assets/"))
            {
                int index = filePath.IndexOf("Assets/", StringComparison.Ordinal);
                filePath = filePath.Substring(index);
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                EditorPrefs.SetString(Application.productName + "_GameEventArgsGenPath", filePath);
                savePath = filePath;
            }
        }

        private void GenerateCode()
        {
            // Ensure the save path is valid
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            
            string directoryPath = Path.Combine(savePath, subdirectory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, $"{className}.cs");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("//------------------------------------------------------------");
                writer.WriteLine("// 此文件由工具自动生成");
                writer.WriteLine("// 生成时间：" + DateTime.Now);
                writer.WriteLine("//------------------------------------------------------------");
                
                writer.WriteLine("using GameFramework;");
                writer.WriteLine("using GameFramework.Event;");
                writer.WriteLine($"namespace {namespaceName}");
                writer.WriteLine("{");
                writer.WriteLine($"    public class {className} : GameEventArgs");
                writer.WriteLine("    {");
                writer.WriteLine("        public static readonly int EventId = typeof(" + className + ").GetHashCode();");
                writer.WriteLine();
                writer.WriteLine("        public override int Id");
                writer.WriteLine("        {");
                writer.WriteLine("            get");
                writer.WriteLine("            {");
                writer.WriteLine("                return EventId;");
                writer.WriteLine("            }");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        public static " + className + " Create(GameFramework.UI.OpenUIFormSuccessEventArgs e)");
                writer.WriteLine("        {");
                writer.WriteLine("            " + className + " eventArgs = ReferencePool.Acquire<" + className + ">();");
                writer.WriteLine("            return eventArgs;");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        public override void Clear()");
                writer.WriteLine("        {");
                writer.WriteLine("            // Clear fields here");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("}");

                Debug.Log($"Code file for {className} generated at {filePath}");
            }

            AssetDatabase.Refresh();
        }
    }
}