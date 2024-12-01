//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-30 12:43:55
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class LevelChooseForm : UGuiForm
    {
        [SerializeField] private float _levelItemWidth;
        private float _originPosX;
        private int _curChooseIndex;
        private List<RectTransform> _levelItems = new();

        private Tween _chooseTween;

        private bool _pendingClose = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
            _originPosX = m_rect_LevelGroup.localPosition.x;
            _curChooseIndex = 0;

            for (int i = 0; i < m_rect_LevelGroup.childCount; i++)
            {
                _levelItems.Add(m_rect_LevelGroup.GetChild(i).GetComponent<RectTransform>());
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _curChooseIndex = Mathf.Min(_levelItems.Count - 1, _curChooseIndex + 1);
                Debug.Log("curChooseIndex: " + _curChooseIndex);
                ChooseIndex(_curChooseIndex);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _curChooseIndex = Mathf.Max(0, _curChooseIndex - 1);
                Debug.Log("curChooseIndex: " + _curChooseIndex);
                ChooseIndex(_curChooseIndex);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !_pendingClose)
            {
                _pendingClose = true;
                GameEntry.Cutscene.PlayCutscene(2);
                GameEntry.Event.Subscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            m_btn_ReturnMenu.onClick.AddListener(OnClickReturnMenu);
            var items = GetComponentsInChildren<LevelChooseItem>();
            foreach (var item in items)
            {
                item.OnOpen();
            }

            GameEntry.Event.Subscribe(ChooseLevelArgs.EventId, OnChooseLevel);
        }

        private void RemoveEvents()
        {
            m_btn_ReturnMenu.onClick.RemoveListener(OnClickReturnMenu);
            var items = GetComponentsInChildren<LevelChooseItem>();
            foreach (var item in items)
            {
                item.OnClose();
            }

            GameEntry.Event.Unsubscribe(ChooseLevelArgs.EventId, OnChooseLevel);
        }

        private void ChooseIndex(int index)
        {
            _curChooseIndex = index;
            float end = _originPosX - (index) * _levelItemWidth;
            Debug.Log(end);
            _levelItems[index].gameObject.SetActive(true);
            if (_chooseTween.IsActivePlaying()) _chooseTween.Kill();
            _chooseTween = DOTween.To(() => m_rect_LevelGroup.anchoredPosition.x, x => m_rect_LevelGroup.SetPosX(x),
                end, 0.5f).SetUpdate(true);
            // _chooseTween.onUpdate += () => Debug.Log(m_rect_LevelGroup.anchoredPosition.x);
            _chooseTween.onComplete += DeactivateInvisibleLevelItems;
        }

        private void DeactivateInvisibleLevelItems()
        {
        }

        #region Events

        private void OnClickReturnMenu()
        {
            if (!_pendingClose)
            {
                _pendingClose = true;
                GameEntry.Cutscene.PlayCutscene(2);
                GameEntry.Event.Subscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);
            }
        }

        private void OnChooseLevel(object sender, GameEventArgs e)
        {
            ChooseLevelArgs args = (ChooseLevelArgs)e;
            // GameEntry.UI.CloseUIForm(this);
            if (GameEntry.Procedure.CurrentProcedure is ProcedureMain)
            {
                GameEntry.UI.CloseUIFormById(UIFormId.PauseForm);
            }

            (GameEntry.Procedure.CurrentProcedure as ProcedureMenu)?.EnterGame(_curChooseIndex + 1);
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain)?.ChooseLevel(_curChooseIndex + 1);
        }

        private void OnCutsceneEnter(object sender, GameEventArgs e)
        {
            _pendingClose = false;
            GameEntry.UI.CloseUIForm(this);
            GameEntry.Event.Unsubscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);
            GameEntry.Cutscene.FadeCutscene();
        }

        #endregion
    }
}