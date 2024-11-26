using System;
using System.Collections;
using Echo;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public abstract class GameEntityBase : MonoBehaviour
    {
        private bool _bSubscribeInitEvent;
        private bool _bInited;


        #region 接管Unity生命周期

        private void Awake()
        {
            if (!GameEntry.Event) return;
            if (GameManager.IsInited)
            {
                _bSubscribeInitEvent = false;
                _bInited = true;
                OnInit();
            }
            else
            {
                _bSubscribeInitEvent = true;
                GameEntry.Event.Subscribe(OnGameManagerInitArg.EventId, OnGameManagerInitEnd);
            }
        }

        private void Start()
        {
            if (!GameEntry.Event) return;
            if (!_bSubscribeInitEvent)
            {
                OnAfterInit();
            }
        }

        protected virtual void OnDestroy()
        {
            if (!GameEntry.Event) return;
            if (_bSubscribeInitEvent)
            {
                if (GameEntry.Event&&GameEntry.Event.Check(OnGameManagerInitArg.EventId, OnGameManagerInitEnd))
                    GameEntry.Event.Unsubscribe(OnGameManagerInitArg.EventId, OnGameManagerInitEnd);
            }
            OnBeDestroyed();
        }

        private void Update()
        {
            if (!_bInited) return;
            OnUpdate();
        }

        private void FixedUpdate()
        {
            if (!_bInited) return;
            OnFixedUpdate();
        }

        private void LateUpdate()
        {
            if (!_bInited) return;
            OnLateUpdate();
        }

        #endregion


        private void OnGameManagerInitEnd(object sender, GameEventArgs e)
        {
            OnInit();
            StartCoroutine(nameof(WaitAllInit));
        }

        private IEnumerator WaitAllInit()
        {
            yield return new WaitForEndOfFrame();
            OnAfterInit();
            _bInited = true;
        }

        #region 子类生命周期

        protected virtual void OnInit()
        {
        }

        protected virtual void OnAfterInit()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnFixedUpdate()
        {
        }

        protected virtual void OnLateUpdate()
        {
        }

        protected virtual void OnBeDestroyed()
        {
        }
        

        #endregion
    }
}