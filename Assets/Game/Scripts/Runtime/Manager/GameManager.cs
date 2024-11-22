using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class GameManager : MonoBehaviour
    {
        #region Static

        public static GameManager Instance { get; private set; }


        #endregion


        public static void Create()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("GameManager");
                Instance = go.AddComponent<GameManager>();
                Instance.OnEnter();
            }
        }

        private List<IManager> _managers = new();
        private List<IUpdatable> _updatables = new();
        private bool Inited = false;
        private int _gameMainFormId;

        private T CreateManager<T>(string name) where T : ManagerBase
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(transform);
            T mgr = obj.AddComponent<T>();
            _managers.Add(mgr);
            if (mgr.TryGetComponent<IUpdatable>(out var updatable))
            {
                _updatables.Add(updatable);
            }

            return mgr;
        }

        public void OnEnter()
        {
            foreach (var mgr in _managers)
            {
                mgr.OnEnter();
            }
        }

        private void OnInitEnd()
        {
            foreach (var mgr in _managers)
            {
                mgr.OnInitEnd();
            }

            _gameMainFormId = (int)GameEntry.UI.OpenUIForm(UIFormId.GameMainForm);
        }

        public void Update()
        {
            if (!Inited)
            {
                foreach (var mgr in _managers)
                {
                    if (!mgr.Inited)
                    {
                        return;
                    }
                }

                Inited = true;
                OnInitEnd();
            }

            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
        }

        public void OnExit()
        {
            GameEntry.UI.CloseUIForm(_gameMainFormId);
            foreach (var mgr in _managers)
            {
                mgr.OnExit();
            }
        }
    }
}