using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor.GFExtension.Editor
{
    public class UIFormCodeGeneratorWindow : EditorWindow
    {
        private string namespaceName = "Party";
        private string className;
        private string savePath;

        [MenuItem("Tools/UGui代码生成器")]
        public static void ShowWindow()
        {
            GetWindow<UIFormCodeGeneratorWindow>("UGui代码生成器");
        }

        private void OnEnable()
        {
            savePath = EditorPrefs.GetString(Application.productName + "_UGuiFormGenPath", "Assets");
        }

        private void OnGUI()
        {
            GUILayout.Label("UGui代码生成器", EditorStyles.boldLabel);

            namespaceName = EditorGUILayout.TextField("Namespace", namespaceName);
            className = EditorGUILayout.TextField("Class Name", className);
            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("路径：",GUILayout.Width(40));
            EditorGUILayout.LabelField(savePath);
            if (GUILayout.Button("选择"))
            {
                ChoosePath();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Generate Code"))
            {
                GenerateCode();
            }
        }

        private void ChoosePath()
        {
            string defaultPath = $"Assets/";
            string filePath = EditorUtility.OpenFolderPanel("选择UGui代码存储位置", defaultPath, "");
            if (filePath.Contains("Assets/"))
            {
                int index = filePath.IndexOf("Assets/", StringComparison.Ordinal);
                filePath = filePath.Substring(index);
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                EditorPrefs.SetString(Application.productName + "_UGuiFormGenPath", filePath);
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

            string filePath = Path.Combine(savePath, $"{className}.cs");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("//------------------------------------------------------------");
                writer.WriteLine("// 此文件由工具自动生成");
                writer.WriteLine("// 生成时间：" + DateTime.Now);
                writer.WriteLine("//------------------------------------------------------------");
                writer.WriteLine($"namespace {namespaceName}");
                writer.WriteLine("{");
                writer.WriteLine($"    public partial class {className} : UGuiForm");
                writer.WriteLine("    {");
                writer.WriteLine("        protected override void OnInit(object userData)");
                writer.WriteLine("        {");
                writer.WriteLine("            base.OnInit(userData);");
                writer.WriteLine("            //GetBindComponents(gameObject);");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        protected override void OnOpen(object userData)");
                writer.WriteLine("        {");
                writer.WriteLine("            base.OnOpen(userData);");
                writer.WriteLine("\t\t\tRegisterEvents();");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        protected override void OnClose(bool isShutdown, object userData)");
                writer.WriteLine("        {");
                writer.WriteLine("            base.OnClose(isShutdown, userData);");
                writer.WriteLine("\t\t\tRemoveEvents();");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        private void RegisterEvents()");
                writer.WriteLine("        {");
                writer.WriteLine("            ");
                writer.WriteLine("        }");
                writer.WriteLine();
                writer.WriteLine("        private void RemoveEvents()");
                writer.WriteLine("        {");
                writer.WriteLine("            ");
                writer.WriteLine("        }");
                writer.WriteLine("    }");
                writer.WriteLine("}");

                Debug.Log($"Code file for {className} generated at {filePath}");
            }

            AssetDatabase.Refresh();
        }
    }
}