using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class UIExtension
    {
        public static bool TryCloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            if (!uiForm || !uiComponent.HasUIForm(uiForm.UIForm.SerialId)) return false;
            uiComponent.CloseUIForm(uiForm.UIForm);
            return true;
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
        
        /// <summary>
        /// 根据数据表中的UIFormId关闭UIForm
        /// </summary>
        /// <param name="uiComponent"></param>
        /// <param name="uiFormId"></param>
        /// <returns></returns>
        public static bool TryCloseUIFormById(this UIComponent uiComponent, int uiFormId)
        {
            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName, drUIForm.UIGroupName);
            if (!uiComponent.HasUIForm(assetName)) return false;
            uiComponent.CloseUIForm(uiComponent.GetUIFormById(uiFormId));
            return true;
        }
    }
}