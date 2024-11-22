using GameFramework;

namespace GameMain
{
    public partial class AssetUtility
    {
        public static string GetScriptableObjectAsset(string assetName)
        {
            return Utility.Text.Format("{0}/ScriptableObjects/{1}.asset", ResRootPath, assetName);
        }

        public static string GetDialogue(string assetName)
        {
            return Utility.Text.Format("{0}/SO/Dialogues/{1}.asset", ResRootPath, assetName);
        }
    }
}