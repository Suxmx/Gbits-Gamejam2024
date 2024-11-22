//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024/11/4 14:08:50
//------------------------------------------------------------

using UnityEngine;

namespace GameMain
{
    public partial class SettingForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEntry.UI.CloseUIForm(this);
            }
        }

        private void RegisterEvents()
        {
        }

        private void RemoveEvents()
        {
        }
    }
}