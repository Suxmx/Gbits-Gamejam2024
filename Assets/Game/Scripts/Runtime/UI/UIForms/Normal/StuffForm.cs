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
        private bool _pendingClose = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            //GetBindComponents(gameObject);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
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
        }

        private void RemoveEvents()
        {
        }

        private void OnCutsceneEnter(object sender, GameEventArgs e)
        {
            _pendingClose = false;
            GameEntry.UI.CloseUIForm(this);
            GameEntry.Event.Unsubscribe(OnCutsceneEnterArgs.EventId, OnCutsceneEnter);
            GameEntry.Cutscene.FadeCutscene();
        }
    }
}