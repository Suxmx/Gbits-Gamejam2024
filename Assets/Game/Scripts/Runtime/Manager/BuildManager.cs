using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameMain
{
    public enum EBuildItem
    {
        Bouncer,
        Cube,
        WindArea,
        Pendulum,
        Dasher,
        Portal,
        OneWayPlatform,
        DasherUp
    }

    public enum EBuildState
    {
        Build,
        Remove
    }

    public class BuildManager : ManagerBase, IUpdatable
    {
        private static Dictionary<Type, EBuildItem> _type2EnumMap = new()
        {
            { typeof(Bouncer), EBuildItem.Bouncer },
            { typeof(BuildCube), EBuildItem.Cube },
            { typeof(WindArea), EBuildItem.WindArea },
            { typeof(Pendulum), EBuildItem.Pendulum },
            { typeof(Dasher), EBuildItem.Dasher },
            { typeof(Portal), EBuildItem.Portal },
            { typeof(OneWayPlatform), EBuildItem.OneWayPlatform },
            { typeof(DasherUp), EBuildItem.DasherUp }
        };

        public EBuildState BuildState { get; private set; } = EBuildState.Build;

        private Stack<BuildItemBase> _buildItemQueue = new();
        private bool bIsBuilding = false;
        private Dictionary<EBuildItem, GameObject> _prefabMap = new();
        private Dictionary<EBuildItem, int> _countMap = new();

        private BuildItemBase _currentBuildItem;
        private BuildItemBase _preRemoveItem;

        public override async Task OnEnter()
        {
            await LoadBuildItemPrefabs();
            foreach (var item in GameManager.Instance.LevelConfig.AvailableBuildItems)
            {
                _countMap[item.Item] = item.Count;
            }

            Inited = true;
        }

        private async Task LoadBuildItemPrefabs()
        {
            var buildItems = (EBuildItem[])System.Enum.GetValues(typeof(EBuildItem));
            foreach (EBuildItem item in buildItems)
            {
                // 创建一个任务并添加到列表
                // Debug.Log("load prefab for " + item);
                _prefabMap[item] = await GameEntry.Resource
                    .LoadAssetAsync<GameObject>(AssetUtility.GetBuildItemPrefab(item));
                // Debug.Log("load prefab for " + item + " done");
            }
        }

        public override void OnInitEnd()
        {
        }

        public override void OnExit()
        {
        }

        private RaycastHit[] _tmpHits = new RaycastHit[10];

        public void OnUpdate(float deltaTime)
        {
            if (BuildState == EBuildState.Build && bIsBuilding && _currentBuildItem != null)
            {
                _currentBuildItem.transform.position = GameManager.MousePosToWorldPlanePos();

                //取消建造
                if (Input.GetMouseButtonDown(1))
                {
                    bIsBuilding = false;
                    Destroy(_currentBuildItem.gameObject);
                    _currentBuildItem = null;
                    return;
                }

                if (_currentBuildItem.DetectBuildable())
                {
                    _currentBuildItem.SetOutlinerColor(new Color(0, 160 / 255f, 0));
                }
                else
                {
                    _currentBuildItem.SetOutlinerColor(Color.red);
                    return;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    var type = _type2EnumMap[_currentBuildItem.GetType()];
                    bIsBuilding = false;
                    _currentBuildItem.SetOutliner(false);
                    _currentBuildItem.EnableLogic();
                    //加入快速删除队列
                    _buildItemQueue.Push(_currentBuildItem);
                    _currentBuildItem = null;
                    _countMap[type]--;
                    GameEntry.Event.Fire(this, OnBuildItemCountChangeArgs.Create(type,
                        _countMap[type]));
                }
            }
            else if (BuildState == EBuildState.Remove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var size = Physics.RaycastNonAlloc(ray, _tmpHits, 1000f,
                    LayerMask.GetMask("SheepIgnore", "Default", "CantBuild", "SheepInteract"),
                    queryTriggerInteraction: QueryTriggerInteraction.Ignore);
                bool found = false;
                for (int i = 0; i < size; i++)
                {
                    var hit = _tmpHits[i];
                    BuildItemBase buildItem = null;
                    //从碰撞体或者父物体中获取BuildItem
                    if (hit.collider.TryGetComponent(out buildItem))
                    {
                        found = true;
                    }
                    else
                    {
                        buildItem = hit.collider.GetComponentInParent<BuildItemBase>();
                        if (buildItem is not null) found = true;
                    }

                    //找到了
                    if (found)
                    {
                        _preRemoveItem.SetOutliner(false);
                        _preRemoveItem = buildItem;
                        // if(!buildItem) continue;
                        buildItem.SetOutliner(true);
                        buildItem.SetOutlinerColor(Color.red);
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (buildItem is Portal portal)
                            {
                                portal.AttachedPortal.Remove();
                            }

                            var type = _type2EnumMap[buildItem.GetType()];
                            buildItem.Remove();
                            _countMap[type]++;
                            GameEntry.Event.Fire(this, OnBuildItemCountChangeArgs.Create(type,
                                _countMap[type]));
                            _preRemoveItem = null;
                        }

                        break;
                    }
                }

                if (!found && _preRemoveItem != null)
                {
                    _preRemoveItem.SetOutliner(false);
                    _preRemoveItem = null;
                }
            }
        }

        public void StartBuild(EBuildItem item)
        {
            ChangeBuildState(EBuildState.Build);
            if (bIsBuilding)
            {
                if (_currentBuildItem)
                {
                    Destroy(_currentBuildItem.gameObject);
                    _currentBuildItem = null;
                }
            }

            if (!(_countMap.ContainsKey(item) && _countMap[item] > 0))
            {
                return;
            }

            if (_prefabMap.TryGetValue(item, out var prefab))
            {
                bIsBuilding = true;
                _currentBuildItem = Instantiate(prefab).GetComponent<BuildItemBase>();
                _currentBuildItem.DisableLogicWhenBuilding();
                _currentBuildItem.SetOutliner(true);
                _currentBuildItem.transform.position = GameManager.MousePosToWorldPlanePos();
            }
            else
            {
                return;
            }
        }

        public void DoFastDelete()
        {
            if (_buildItemQueue.Count <= 0) return;

            var item = _buildItemQueue.Pop();
            //如果已经被删除了，就继续删除下一个
            if (item == null)
            {
                DoFastDelete();
                return;
            }

            if (item is Portal portal)
            {
                portal.AttachedPortal.Remove();
            }

            item.Remove();
            var type = _type2EnumMap[item.GetType()];
            _countMap[type]++;
            GameEntry.Event.Fire(this, OnBuildItemCountChangeArgs.Create(type,
                _countMap[type]));
        }

        public void ChangeBuildState(EBuildState state)
        {
            if (BuildState == state) return;
            BuildState = state;
            GameEntry.Event.Fire(this, OnBuildStateChangeArgs.Create(state));
        }

        private struct BuildItemSaveData
        {
            public BuildItemBase Item;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        List<BuildItemSaveData> _buildItemSaveDatas = new();

        public void SaveBuildItemStates()
        {
            if (_currentBuildItem)
            {
                Destroy(_currentBuildItem.gameObject);
                bIsBuilding = false;
            }

            _buildItemSaveDatas.Clear();
            var items = FindObjectsByType<BuildItemBase>(FindObjectsSortMode.None);
            foreach (var item in items)
            {
                var data = new BuildItemSaveData()
                {
                    Item = item,
                    Position = item.transform.position,
                    Rotation = item.transform.rotation
                };
                _buildItemSaveDatas.Add(data);
            }
        }

        public void ResumeBuildItemStates()
        {
            foreach (var data in _buildItemSaveDatas)
            {
                data.Item.transform.position = data.Position;
                data.Item.transform.rotation = data.Rotation;
                var rigid = data.Item.GetComponent<Rigidbody>();
                if (rigid)
                {
                    rigid.linearVelocity = Vector3.zero;
                    rigid.angularVelocity = Vector3.zero;
                }
            }
        }
    }
}