//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-12-01 13:18:04
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class PauseForm : UGuiForm
    {
        [SerializeField] private Color _chosenFontColor;
        private List<Button> _btns = new();
        private int _curBtnIndex = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
            _btns.Add(m_btn_Resume);
            _btns.Add(m_btn_LevelSelect);
            _btns.Add(m_btn_ReturnMenu);
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
                _curBtnIndex = _curBtnIndex - 1 < 0 ? 2 : _curBtnIndex - 1;
                PreSelectButton(_curBtnIndex);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                _curBtnIndex = _curBtnIndex + 1 > 2 ? 0 : _curBtnIndex + 1;
                PreSelectButton(_curBtnIndex);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                DoChooseBtn();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickResume();
            }
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
            s.Append(_btns[index].transform.DOScale(new Vector3(0.9f, 1.1f, 1), 0.1f).SetUpdate(true));
            s.Append(_btns[index].transform.DOScale(new Vector3(1f, 1f, 1), 0.1f).SetUpdate(true));
            s.SetUpdate(true).Play();

            transform.DOShakePosition(0.15f, new Vector3(0, 5, 0), 10, 90, false,
                true).SetUpdate(true).onComplete = () => transform.localPosition = Vector3.zero;
        }

        private void DoChooseBtn()
        {
            switch (_curBtnIndex)
            {
                case 0:
                    OnClickResume();
                    break;
                case 1:
                    OnClickLevelSelect();
                    break;
                case 2:
                    OnClickReturnMenu();
                    break;
            }
        }

        private void OnHoverButton(RectTransform rect)
        {
            int index = _btns.FindIndex(x => x.transform == rect);
            if (index == -1 || index == _curBtnIndex) return;
            _curBtnIndex = index;
            PreSelectButton(_curBtnIndex);
        }

        private void RegisterEvents()
        {
            m_btn_LevelSelect.onClick.AddListener(OnClickLevelSelect);
            m_btn_Resume.onClick.AddListener(OnClickResume);
            m_btn_ReturnMenu.onClick.AddListener(OnClickReturnMenu);
        }

        private void RemoveEvents()
        {
            m_btn_LevelSelect.onClick.RemoveListener(OnClickLevelSelect);
            m_btn_Resume.onClick.RemoveListener(OnClickResume);
            m_btn_ReturnMenu.onClick.RemoveListener(OnClickReturnMenu);
        }

        private void OnClickResume()
        {
            GameEntry.UI.CloseUIForm(this);
            GameManager.Instance.Pause = false;
        }

        private void OnClickLevelSelect()
        {
            GameEntry.UI.OpenUIForm(UIFormId.LevelChooseForm);
        }

        private void OnClickReturnMenu()
        {
            GameMain.GameEntry.UI.CloseUIForm(this);
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain)?.ReturnMenu();
        }
    }
}