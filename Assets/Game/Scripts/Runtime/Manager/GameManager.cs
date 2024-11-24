using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Echo;
using UnityEngine;

namespace GameMain
{
    public class GameManager : MonoBehaviour
    {
        #region Static

        public static GameManager Instance { get; private set; }
        public static Plane BuildPlane = new Plane(Vector3.forward, new Vector3(0, 0, 23.47f));

        public static BuildManager Build { get; private set; }
        public static bool IsInited => Instance != null && Instance.Inited;

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

        public int ArriveSheepCount = 0;

        private List<IManager> _managers = new();
        private List<IUpdatable> _updatables = new();
        private List<IFixedUpdatable> _fixedUpdatables = new();
        private List<ILateUpdatable> _lateUpdatables = new();
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

            if (mgr.TryGetComponent<IFixedUpdatable>(out var lateUpdatable))
            {
                _fixedUpdatables.Add(lateUpdatable);
            }

            if (mgr.TryGetComponent<ILateUpdatable>(out var fixedUpdatable))
            {
                _lateUpdatables.Add(fixedUpdatable);
            }

            return mgr;
        }

        public void OnEnter()
        {
            Build = CreateManager<BuildManager>("Build");
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

            //打开主界面
            _gameMainFormId = (int)GameEntry.UI.OpenUIForm(UIFormId.GameMainForm);
            GameEntry.Event.FireNow(this, OnGameManagerInitArg.Create());
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
                updatable.OnUpdate(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (!Inited)
            {
                return;
            }

            foreach (var updatable in _lateUpdatables)
            {
                updatable.OnLateUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (!Inited)
            {
                return;
            }

            foreach (var updatable in _fixedUpdatables)
            {
                updatable.OnFixedUpdate(Time.deltaTime);
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

        public static Vector3 MousePosToWorldPlanePos()
        {
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if (BuildPlane.Raycast(ray, out float distance))
            {
                // 获取交点
                Vector3 worldPosition = ray.GetPoint(distance);
                return worldPosition;
            }

            return new Vector3(0, 0, 23.47f);
        }
    }
}