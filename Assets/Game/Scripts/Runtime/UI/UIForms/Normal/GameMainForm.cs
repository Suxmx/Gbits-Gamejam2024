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
        [SerializeField] private Texture2D _normalCursor;
        [SerializeField] private Sprite _editModeSprite;
        [SerializeField] private Sprite _playModeSprite;
        [SerializeField] private Dictionary<EBuildItem, GameObject> _buildItemPrefabMap = new();

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickPause();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameEntry.UI.OpenUIForm(UIFormId.SettleUIForm);
            }
        }

        private void RefreshStateOnOpen()
        {
            m_rect_PauseMenu.gameObject.SetActive(false);

            foreach (var config in GameManager.Instance.LevelConfig.AvailableBuildItems)
            {
                var buildItem = Instantiate(_buildItemPrefabMap[config.Item], m_rect_BuildItem)
                    .GetComponent<BuildItemUI>();
                buildItem.Init(config.Item, config.Count);
            }

            ChangeBuildStateUI(GameManager.Build.BuildState);
            //底部工具栏
            m_btn_ExitPlay.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Runtime);
            m_btn_Start.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
            m_btn_Undo.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
            m_btn_Remove.gameObject.SetActive(GameManager.Instance.GameState == EGameState.Editor);
            //现有羊数量
            m_tmp_ArriveSheep.text =
                $"到达羊数量：{GameManager.Instance.ArriveSheepCount}/{GameManager.Instance.TotalSheepCount}";

            m_img_Mode.sprite = GameManager.Instance.GameState == EGameState.Editor ? _editModeSprite : _playModeSprite;
        }

        #region Events

        private void RegisterEvents()
        {
            //bottom bar
            m_btn_Remove.onClick.AddListener(OnClickRemove);
            m_btn_ExitPlay.onClick.AddListener(OnClickExitPlay);
            //top bar
            m_btn_Restart.onClick.AddListener(OnClickRestart);
            m_btn_Start.onClick.AddListener(OnClickStart);
            m_btn_Undo.onClick.AddListener(OnClickUndo);
            //pause menu
            m_btn_ReturnMenu.onClick.AddListener(OnClickReturnMenu);
            m_btn_ResumeGame.onClick.AddListener(OnClickResumeGame);

            GameEntry.Event.Subscribe(OnBuildStateChangeArgs.EventId, OnBuildStateChange);
            GameEntry.Event.Subscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
            GameEntry.Event.Subscribe(SheepArriveArgs.EventId, OnSheepArrive);
        }

        private void RemoveEvents()
        {
            //bottom bar
            m_btn_Remove.onClick.RemoveListener(OnClickRemove);
            m_btn_ExitPlay.onClick.RemoveListener(OnClickExitPlay);
            //top bar
            m_btn_Restart.onClick.RemoveListener(OnClickRestart);
            m_btn_Start.onClick.RemoveListener(OnClickStart);
            m_btn_Undo.onClick.RemoveListener(OnClickUndo);
            //pause menu
            m_btn_ReturnMenu.onClick.RemoveListener(OnClickReturnMenu);
            m_btn_ResumeGame.onClick.RemoveListener(OnClickResumeGame);

            GameEntry.Event.Unsubscribe(OnBuildStateChangeArgs.EventId, OnBuildStateChange);
            GameEntry.Event.Unsubscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
            GameEntry.Event.Unsubscribe(SheepArriveArgs.EventId, OnSheepArrive);
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

        private void OnClickExitPlay()
        {
            GameManager.Instance.ChangeGameState(EGameState.Editor);
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
            m_btn_ExitPlay.gameObject.SetActive(arg.GameState == EGameState.Runtime);
            m_btn_Start.gameObject.SetActive(arg.GameState == EGameState.Editor);
            m_btn_Undo.gameObject.SetActive(arg.GameState == EGameState.Editor);
            m_btn_Remove.gameObject.SetActive(arg.GameState == EGameState.Editor);

            m_img_Mode.sprite = arg.GameState == EGameState.Editor ? _editModeSprite : _playModeSprite;
        }

        private void OnSheepArrive(object sender, GameEventArgs e)
        {
            var arg = (SheepArriveArgs)e;
            m_tmp_ArriveSheep.text = $"到达羊数量：{arg.ArriveSheepCount}/{arg.TotalSheepCount}";
        }

        #endregion

        public void ChangeBuildStateUI(EBuildState state)
        {
            if (state == EBuildState.Remove)
            {
                Cursor.SetCursor(_removeCursor, new Vector2(16, 16), CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(_normalCursor, new Vector2(16, 16), CursorMode.Auto);
            }
        }
    }
}