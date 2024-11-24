using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameMain
{
    public enum EBuildItem
    {
        Bouncer,
        Cube,
    }

    public class BuildManager : ManagerBase, IUpdatable
    {
        private bool bIsBuilding = false;
        private Dictionary<EBuildItem, GameObject> _prefabMap = new();
        private BuildItemBase _currentBuildItem;

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

        public void OnUpdate(float deltaTime)
        {
            if (bIsBuilding)
            {
                _currentBuildItem.transform.position = GameManager.MousePosToWorldPlanePos();
                if (Input.GetMouseButtonDown(0))
                {
                    bIsBuilding = false;
                    _currentBuildItem.EnableLogic();
                    _currentBuildItem = null;
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
                _currentBuildItem  = Instantiate(prefab).GetComponent<BuildItemBase>();
                _currentBuildItem.DisableLogicWhenBuilding();
                _currentBuildItem.transform.position = GameManager.MousePosToWorldPlanePos();
            }
            else
            {
                return;
            }
        }
    }
}