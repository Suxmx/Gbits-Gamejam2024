//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-12-01 00:54:22
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class StuffForm : UGuiForm
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

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DoClose();
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            m_btn_ReturnMenu.onClick.AddListener(DoClose);
        }

        private void RemoveEvents()
        {
            m_btn_ReturnMenu.onClick.RemoveListener(DoClose);
        }

        private void DoClose()
        {
            GameEntry.Cutscene.PlayCutscene(OnCutsceneEnter, 2);
        }


        private void OnCutsceneEnter()
        {
            GameEntry.UI.TryCloseUIForm(this);
            GameEntry.Cutscene.FadeCutscene(null, 2);
        }
    }
}