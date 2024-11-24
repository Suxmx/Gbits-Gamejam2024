//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024/11/4 14:27:29
//------------------------------------------------------------

using UnityEngine;

namespace GameMain
{
    public partial class GameMainForm : UGuiForm
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
            if (GameEntry.Debugger is not null)
            {
                m_txt_FrameRate.text = $"帧率：{(GameEntry.Procedure.CurrentProcedure as ProcedureMain).CurrentFps,0:00}";
            }
        }

        private void RegisterEvents()
        {
            m_btn_ExitGame.onClick.AddListener(ReturnMenu);
        }

        private void RemoveEvents()
        {
            m_btn_ExitGame.onClick.RemoveListener(ReturnMenu);
        }

        private void ReturnMenu()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain).ReturnMenu();
        }
    }
}