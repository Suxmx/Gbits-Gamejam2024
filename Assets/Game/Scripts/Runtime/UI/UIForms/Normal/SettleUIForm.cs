//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-12-01 07:04:00
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public partial class SettleUIForm : UGuiForm
    {
        private Animator _animator;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GetBindComponents(gameObject);
            _animator = m_rect_Circle.GetComponent<Animator>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
            _animator.Play("Expand");
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
    }
}