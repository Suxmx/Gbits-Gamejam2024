#if UNITY_EDITOR

using UnityEngine;
using UnityEditor; // 需要编辑器相关功能
using Sirenix.OdinInspector;
using System.IO;

namespace Game
{
    public class TransformRendertexture : MonoBehaviour
    {
        [SerializeField] RenderTexture renderTexture;

        [Button("Save RenderTexture As PNG")]
        public void SaveRenderTextureToPNG()
        {
            if (renderTexture == null)
            {
                Debug.LogError("RenderTexture is null!");
                return;
            }

            // 弹出保存文件对话框
            string path = EditorUtility.SaveFilePanel(
                "Save Texture as PNG",
                Application.dataPath + "Game\\Res\\Images\\UI\\BuildItem",
                "RenderTexture.png",
                "png");

            // 如果用户取消保存，直接返回
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Save operation canceled.");
                return;
            }

            // 转换 RenderTexture 为 Texture2D
            Texture2D texture = ConvertRenderTextureToTexture2D(renderTexture);

            // 将 Texture2D 保存为 PNG 文件
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes(path, pngData);

            Debug.Log($"Texture saved to {path}");
            AssetDatabase.Refresh(); // 刷新资源数据库
        }

        // 将 RenderTexture 转换为 Texture2D 的静态方法
        private static Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTexture)
        {
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

            // 激活 RenderTexture
            RenderTexture.active = renderTexture;

            // 读取像素并进行色彩空间转换
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            // 如果你的项目使用 Linear 色彩空间，执行这一步
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                Color[] pixels = texture.GetPixels();
                for (int i = 0; i < pixels.Length; i++)
                {
                    pixels[i] = pixels[i].gamma; // 转换为 Gamma 空间
                }

                texture.SetPixels(pixels);
            }

            texture.Apply();
            RenderTexture.active = null;

            return texture;
        }
    }
}
#endif