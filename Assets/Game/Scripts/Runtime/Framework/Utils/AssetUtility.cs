using GameFramework;

namespace GameMain
{
    public static partial class AssetUtility
    {
        public static string MenuSceneName = "Menu";

        // public static string MainSceneName = "Main";
        public static string SplashSceneName = "Splash";
        public static string DialogueSceneName = "Dialogue";

        public static string ResRootPath = "Assets/Game/Res";

        public static string GetConfigAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("{0}/Configs/{1}.{2}", ResRootPath, assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDataTableAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("{0}/DataTables/{1}.{2}", ResRootPath, assetName, fromBytes ? "bytes" : "txt");
        }

        public static string GetDictionaryAsset(string assetName, bool fromBytes)
        {
            return Utility.Text.Format("{0}/Localization/{1}/Dictionaries/{2}.{3}",
                ResRootPath, GameEntry.Localization.Language, assetName, fromBytes ? "bytes" : "xml");
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Scenes/{1}.unity", ResRootPath, assetName);
        }

        public static string GetUIFormAsset(string assetName, string groupName)
        {
            return Utility.Text.Format("{0}/UI/UIForms/{1}/{2}.prefab", ResRootPath, groupName, assetName);
        }

        public static string GetDialogueAsset(string assetName, bool fromJson = false)
        {
            return Utility.Text.Format("{0}/ScriptableObjects/{1}.{2}", ResRootPath, assetName,
                fromJson ? "json" : "asset");
        }

        public static string GetFontSDFAsset(string assetName)
        {
            return Utility.Text.Format("{0}/Fonts/{1}.asset", ResRootPath, assetName);
        }

        public static string GetBuildItemPrefab(EBuildItem item)
        {
            return Utility.Text.Format("{0}/Prefabs/BuildItems/{1}.prefab", ResRootPath, item.ToString());
        }

        public static string GetLevelSceneSubName(int levelIndex)
        {
            return Utility.Text.Format("Levels/Level{0}", levelIndex);
        }
    }
}