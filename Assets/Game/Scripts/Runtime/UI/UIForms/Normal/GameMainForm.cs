//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024/11/4 14:27:29
//------------------------------------------------------------

using System.Collections.Generic;
using GameFramework.Event;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class GameMainForm : UGuiForm
    {
        [SerializeField] private Texture2D _removeCursor;
        [SerializeField] private GameObject _buildItemBtnPrefab;
        [SerializeField] private Dictionary<EBuildItem, Sprite> _buildItemIconMap = new();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RefreshStateOnOpen();
            RegisterEvents();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
            for (int i = 0; i < m_rect_BuildItem.childCount; i++)
            {
                Destroy(m_rect_BuildItem.GetChild(i).gameObject);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (GameEntry.Debugger is not null)
            {
                m_txt_FrameRate.text = $"帧率：{(GameEntry.Procedure.CurrentProcedure as ProcedureMain).CurrentFps,0:00}";
            }
        }

        private void RefreshStateOnOpen()
        {
            m_rect_PauseMenu.gameObject.SetActive(false);

            foreach (var config in GameManager.Instance.LevelConfig.AvailableBuildItems)
            {
                var buildItem = Instantiate(_buildItemBtnPrefab, m_rect_BuildItem).GetComponent<BuildItemUI>();
                buildItem.Init(config.Item, _buildItemIconMap[config.Item]);
            }

            ChangeBuildStateUI(GameManager.Build.BuildState);
            m_btn_Start.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
            m_btn_Undo.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
            m_btn_Remove.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
        }

        #region Events

        private void RegisterEvents()
        {
            //bottom bar
            m_btn_Build.onClick.AddListener(OnClickBuild);
            m_btn_Remove.onClick.AddListener(OnClickRemove);
            //top bar
            m_btn_Pause.onClick.AddListener(OnClickPause);
            m_btn_Restart.onClick.AddListener(OnClickRestart);
            m_btn_Start.onClick.AddListener(OnClickStart);
            m_btn_Undo.onClick.AddListener(OnClickUndo);
            //pause menu
            m_btn_ReturnMenu.onClick.AddListener(OnClickReturnMenu);
            m_btn_ResumeGame.onClick.AddListener(OnClickResumeGame);

            GameEntry.Event.Subscribe(OnBuildStateChangeArgs.EventId, OnBuildStateChange);
            GameEntry.Event.Subscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
        }

        private void RemoveEvents()
        {
            //bottom bar
            m_btn_Build.onClick.RemoveListener(OnClickBuild);
            m_btn_Remove.onClick.RemoveListener(OnClickRemove);
            //top bar
            m_btn_Pause.onClick.RemoveListener(OnClickPause);
            m_btn_Restart.onClick.RemoveListener(OnClickRestart);
            m_btn_Start.onClick.RemoveListener(OnClickStart);
            m_btn_Undo.onClick.RemoveListener(OnClickUndo);
            //pause menu
            m_btn_ReturnMenu.onClick.RemoveListener(OnClickReturnMenu);
            m_btn_ResumeGame.onClick.RemoveListener(OnClickResumeGame);

            GameEntry.Event.Unsubscribe(OnBuildStateChangeArgs.EventId, OnBuildStateChange);
            GameEntry.Event.Unsubscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
        }

        private void OnClickPause()
        {
            var enable = m_rect_PauseMenu.gameObject.activeSelf;
            m_rect_PauseMenu.gameObject.SetActive(!enable);
            GameManager.Instance.Pause = !enable;
        }

        private void OnClickResumeGame()
        {
            m_rect_PauseMenu.gameObject.SetActive(false);
            GameManager.Instance.Pause = false;
        }

        private void OnClickBuild()
        {
            GameManager.Build.ChangeBuildState(EBuildState.Build);
        }

        private void OnClickRemove()
        {
            GameManager.Build.ChangeBuildState(EBuildState.Remove);
        }


        private void OnClickReturnMenu()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain).ReturnMenu();
        }

        private void OnClickStart()
        {
            m_btn_Start.gameObject.SetActive(false);
            GameManager.Instance.ChangeGameState(EGameState.Runtime);
        }

        private void OnClickRestart()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain).Restart();
        }

        private void OnClickUndo()
        {
            GameManager.Build.DoFastDelete();
        }

        //GF events

        private void OnBuildStateChange(object sender, GameEventArgs e)
        {
            var arg = (OnBuildStateChangeArgs)e;
            ChangeBuildStateUI(arg.BuildState);
        }

        private void OnGameStateChange(object sender, GameEventArgs e)
        {
            var arg = (OnGameStateChangeArgs)e;
            if (arg.GameState == EGameState.Runtime)
            {
                m_btn_Undo.gameObject.SetActive(false);
                m_btn_Remove.gameObject.SetActive(false);
            }
        }

        #endregion

        public void ChangeBuildStateUI(EBuildState state)
        {
            m_btn_Build.GetComponent<Image>().color =
                state == EBuildState.Build ? Color.green : Color.white;
            m_btn_Remove.GetComponent<Image>().color =
                state == EBuildState.Remove ? Color.green : Color.white;

            m_rect_BuildItem.gameObject.SetActive(state == EBuildState.Build);
            if (state == EBuildState.Remove)
            {
                Cursor.SetCursor(_removeCursor, new Vector2(16, 16), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}