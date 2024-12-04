using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class MenuForm : UGuiForm
    {
        [SerializeField] private Color _chosenFontColor;
        private int _curBtnIndex = 0;

        private List<Button> _btns = new();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
            _btns.Add(m_btn_StartGame);
            _btns.Add(m_btn_LevelSelect);
            _btns.Add(m_btn_Stuff);
            _btns.Add(m_btn_ExitGame);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponent<ButtonHover>())
                    {
                        OnHoverButton(result.gameObject.transform as RectTransform);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _curBtnIndex = _curBtnIndex - 1 < 0 ? 3 : _curBtnIndex - 1;
                PreSelectButton(_curBtnIndex);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _curBtnIndex = _curBtnIndex + 1 > 3 ? 0 : _curBtnIndex + 1;
                PreSelectButton(_curBtnIndex);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                DoChooseBtn();
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            m_btn_StartGame.onClick.AddListener(OnClickStartGame);
            m_btn_LevelSelect.onClick.AddListener(OnClickLevelSelect);
            m_btn_ExitGame.onClick.AddListener(OnClickExitGame);
            m_btn_Stuff.onClick.AddListener(OnClickStuff);
        }

        private void RemoveEvents()
        {
            m_btn_StartGame.onClick.RemoveListener(OnClickStartGame);
            m_btn_LevelSelect.onClick.RemoveListener(OnClickLevelSelect);
            m_btn_ExitGame.onClick.RemoveListener(OnClickExitGame);
            m_btn_Stuff.onClick.RemoveListener(OnClickStuff);
        }

        private void PreSelectButton(int index)
        {
            for (int i = 0; i < _btns.Count; i++)
            {
                _btns[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                _btns[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1f);
            }

            _btns[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            _btns[index].GetComponentInChildren<TextMeshProUGUI>().color = _chosenFontColor;
            Sequence s = DOTween.Sequence();
            s.Append(_btns[index].transform.DOScale(new Vector3(0.9f, 1.1f, 1), 0.1f));
            s.Append(_btns[index].transform.DOScale(new Vector3(1f, 1f, 1), 0.1f));
            s.Play();

            transform.DOShakePosition(0.15f, new Vector3(0, 5, 0), 10, 90, false,
                true).onComplete = () => transform.localPosition = Vector3.zero;
        }

        private void DoChooseBtn()
        {
            switch (_curBtnIndex)
            {
                case 0:
                    OnClickStartGame();
                    break;
                case 1:
                    OnClickLevelSelect();
                    break;
                case 2:
                    OnClickStuff();
                    break;
                case 3:
                    OnClickExitGame();
                    break;
            }
        }

        #region Events

        private void OnClickStartGame()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMenu)?.EnterGame(1);
        }

        private void OnClickLevelSelect()
        {
            GameEntry.Cutscene.PlayCutscene(OnLevelSelectCutsceneEnter, 2);
        }

        private void OnClickStuff()
        {
            GameEntry.Cutscene.PlayCutscene(OnStuffCutsceneEnter, 2);
        }

        private void OnClickExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        private void OnHoverButton(RectTransform rect)
        {
            int index = _btns.FindIndex(x => x.transform == rect);
            if (index == -1 || index == _curBtnIndex) return;
            _curBtnIndex = index;
            PreSelectButton(_curBtnIndex);
        }


        private void OnStuffCutsceneEnter()
        {
            GameEntry.UI.OpenUIForm(UIFormId.StuffForm);
            GameEntry.Cutscene.FadeCutscene(null);
        }

        private void OnLevelSelectCutsceneEnter()
        {
            GameEntry.UI.OpenUIForm(UIFormId.LevelChooseForm);
            GameEntry.Cutscene.FadeCutscene(null);
        }

        #endregion
    }
}