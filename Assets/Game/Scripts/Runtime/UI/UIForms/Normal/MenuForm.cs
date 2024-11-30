using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace GameMain
{
    public partial class MenuForm : UGuiForm
    {
        private int? _settingFormSerialId = null;
        private int? _levelChooseFormSerialId = null;
        
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
        
        private void RegisterEvents()
        {
            m_btn_StartGame.onClick.AddListener(OnStartGame);
            m_btn_ExitGame.onClick.AddListener(OnExitGame);
            m_btn_Setting.onClick.AddListener(OnOpenSetting);
        }

        private void RemoveEvents()
        {
            m_btn_StartGame.onClick.RemoveListener(OnStartGame);
            m_btn_ExitGame.onClick.RemoveListener(OnExitGame);
            m_btn_Setting.onClick.RemoveListener(OnOpenSetting);
        }

        #region Events

        private void OnStartGame()
        {
            GameEntry.UI.OpenUIForm(UIFormId.LevelChooseForm);
            // (GameEntry.Procedure.CurrentProcedure as ProcedureMenu).EnterGame();
        }

        private void OnExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        private void OnOpenSetting()
        {
            _settingFormSerialId = GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        #endregion
    }
}