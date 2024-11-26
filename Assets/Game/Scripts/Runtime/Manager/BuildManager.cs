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
        Portal
    }

    public enum EBuildState
    {
        Build,
        Remove
    }

    public class BuildManager : ManagerBase, IUpdatable
    {
        public EBuildState BuildState { get; private set; } = EBuildState.Build;

        private Stack<BuildItemBase> _buildItemQueue = new();
        private bool bIsBuilding = false;
        private Dictionary<EBuildItem, GameObject> _prefabMap = new();
        private BuildItemBase _currentBuildItem;
        private BuildItemBase _preRemoveItem;

        public override async Task OnEnter()
        {
            await LoadBuildItemPrefabs();
            Inited = true;
        }

        private Task LoadBuildItemPrefabs()
        {
            var buildItems = (EBuildItem[])System.Enum.GetValues(typeof(EBuildItem));
            var tasks = new List<Task>();
            foreach (EBuildItem item in buildItems)
            {
                // 创建一个任务并添加到列表
                var loadTask = GameEntry.Resource
                    .LoadAssetAsync<GameObject>(AssetUtility.GetBuildItemPrefab(item))
                    .ContinueWith(task =>
                    {
                        if (task.Status == TaskStatus.RanToCompletion)
                        {
                            _prefabMap[item] = task.Result;
                        }
                        else if (task.Status == TaskStatus.Faulted)
                        {
                            Debug.LogError($"Failed to load build item for {item}: {task.Exception?.Message}");
                        }
                    });

                tasks.Add(loadTask);
            }

            return Task.WhenAll(tasks);
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
            if (BuildState == EBuildState.Build && bIsBuilding)
            {
                _currentBuildItem.transform.position = GameManager.MousePosToWorldPlanePos();
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
                    bIsBuilding = false;
                    _currentBuildItem.SetOutliner(false);
                    _currentBuildItem.EnableLogic();
                    //加入快速删除队列
                    _buildItemQueue.Push(_currentBuildItem);
                    _currentBuildItem = null;
                }
            }
            else if (BuildState == EBuildState.Remove)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var size = Physics.RaycastNonAlloc(ray, _tmpHits, 1000f);
                bool found = false;
                for (int i = 0; i < size; i++)
                {
                    var hit = _tmpHits[i];
                    BuildItemBase buildItem = null;
                    if (hit.collider.TryGetComponent(out buildItem))
                    {
                        found = true;
                    }
                    else
                    {
                        buildItem = hit.collider.GetComponentInParent<BuildItemBase>();
                        if (buildItem is not null) found = true;
                    }

                    if (found)
                    {
                        _preRemoveItem = buildItem;
                        buildItem.SetOutliner(true);
                        buildItem.SetOutlinerColor(Color.red);
                        if (Input.GetMouseButtonDown(0))
                        {
                            buildItem.Remove();
                            _preRemoveItem = null;
                        }
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
            if (bIsBuilding)
            {
                if (_currentBuildItem)
                {
                    Destroy(_currentBuildItem.gameObject);
                    _currentBuildItem = null;
                }

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

            item.Remove();
        }

        public void ChangeBuildState(EBuildState state)
        {
            if (BuildState == state) return;
            BuildState = state;
            GameEntry.Event.Fire(this, OnBuildStateChangeArgs.Create(state));
        }
    }
}