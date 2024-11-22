using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class UIExtension
    {
        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName, drUIForm.UIGroupName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, AssetPriority.UIFormAsset,
                drUIForm.PauseCoveredUIForm, userData);
        }

        public static void CloseUIForm(this UIComponent uiComponent, int? uiFormId, object userData = null)
        {
            if (uiFormId is null) return;
            if (!uiComponent.HasUIForm((int)uiFormId)) return;
            uiComponent.CloseUIForm((int)uiFormId, userData);
        }

        public static bool HasUIFormById(this UIComponent uiComponent, int uiFormId)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName, drUIForm.UIGroupName);
            return uiComponent.HasUIForm(assetName);
        }

        public static UIForm GetUIFormById(this UIComponent uiComponent, int uiFormId)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName, drUIForm.UIGroupName);
            return uiComponent.GetUIForm(assetName);
        }

        /// <summary>
        /// 根据数据表中的UIFormId关闭UIForm
        /// </summary>
        /// <param name="uiComponent"></param>
        /// <param name="uiFormId"></param>
        /// <returns></returns>
        public static void CloseUIFormById(this UIComponent uiComponent, int uiFormId)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName, drUIForm.UIGroupName);
            uiComponent.CloseUIForm(uiComponent.GetUIFormById(uiFormId));
        }
    }
}