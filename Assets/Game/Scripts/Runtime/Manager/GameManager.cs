using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Echo;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public enum EGameState
    {
        Editor,
        Runtime
    }

    public class GameManager : MonoBehaviour
    {
        #region Static

        public static GameManager Instance { get; private set; }
        public static Plane BuildPlane = new Plane(Vector3.forward, new Vector3(0, 0, 0));

        public static BuildManager Build { get; private set; }
        public static bool IsInited => Instance != null && Instance.Inited;

        #endregion


        public static void Create(int index)
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("GameManager");
                Instance = go.AddComponent<GameManager>();
                Instance.Level = index;
                _ = Instance.OnEnter();
            }
        }

        public bool Pause
        {
            get => _pause;
            set
            {
                if (_pause == value) return;
                _pause = value;
                GameEntry.Base.GameSpeed = _pause ? 0 : 1;
                GameEntry.Event.Fire(this, OnPauseStateChangeArgs.Create(_pause));
            }
        }

        public EGameState GameState { get; private set; }
        public int Level = 1;

        public int ArriveSheepCount
        {
            get => _arriveSheepCount;
            set
            {
                _arriveSheepCount = value;
                GameEntry.Event.Fire(this, SheepArriveArgs.Create(_arriveSheepCount, TotalSheepCount));
                if (ArriveSheepCount == TotalSheepCount)
                {
                    OnGameSettle();
                }
            }
        }

        public int TotalSheepCount
        {
            get => _totalSheepCount;
            set
            {
                // Debug.Log("set total to " + value);
                _totalSheepCount = value;
                GameEntry.Event.Fire(this, SheepArriveArgs.Create(_arriveSheepCount, TotalSheepCount));
            }
        }

        private int _totalSheepCount = 0;
        private int _arriveSheepCount = 0;
        public LevelConfig LevelConfig;

        private bool _pause = false;
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

        public async Task OnEnter()
        {
            Build = CreateManager<BuildManager>("Build");
            LevelConfig =
                (await GameEntry.Resource.LoadAssetAsync<LevelConfigsSO>(
                    AssetUtility.GetScriptableObjectAsset("LevelConfigs"))).LevelConfigs.Find(x => x.Index == Level);
            foreach (var mgr in _managers)
            {
                await mgr.OnEnter();
            }
        }

        private void OnInitEnd()
        {
            foreach (var mgr in _managers)
            {
                mgr.OnInitEnd();
            }

            GameState = EGameState.Editor;
            //退出cutscene
            if (GameEntry.Cutscene.IsPlayingCutscene)
            {
                GameEntry.Cutscene.FadeCutscene(OnCutsceneFade);
            }
            else
            {
                OnCutsceneFade();
            }
        }

        private void OnCutsceneFade()
        {
            if (Level == 1)
            {
                var animHelper = FindAnyObjectByType<Level1AnimHelper>();
                if (animHelper)
                {
                    animHelper.Play();
                }
                else
                {
                    ShowUIFormAndInitEnd();
                }
            }
            else
            {
                ShowUIFormAndInitEnd();
            }
        }

        public void ShowUIFormAndInitEnd()
        {
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

        private void OnGameSettle()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettleUIForm);
        }

        public static Vector3 MousePosToWorldPlanePos()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (BuildPlane.Raycast(ray, out float distance))
            {
                // 获取交点
                Vector3 worldPosition = ray.GetPoint(distance);
                return worldPosition;
            }

            return new Vector3(0, 0, 23.47f);
        }

        public void ChangeGameState(EGameState state)
        {
            if (state == GameState) return;
            GameState = state;
            if (GameState == EGameState.Runtime)
            {
                Build.ChangeBuildState(EBuildState.Build);
                Build.SaveBuildItemStates();
            }
            else
            {
                ArriveSheepCount = 0;
                Build.ResumeBuildItemStates();
                var sheep = FindObjectsByType<Sheep>(FindObjectsSortMode.None);
                for (int i = sheep.Length - 1; i >= 0; i--)
                {
                    Destroy(sheep[i].gameObject);
                }
            }

            GameEntry.Event.Fire(this, OnGameStateChangeArgs.Create(state));
        }
    }
}