using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SheepSpawner : GameEntityBase
    {
        [SerializeField] private GameObject _sheepPrefab;
        [SerializeField] private int _sheepCount = 10;
        [SerializeField] private float _spawnInterval = 1;
        private bool _start = false;

        private float _timer = 0;

        protected override void OnInit()
        {
            GameEntry.Event.Subscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
            GameManager.Instance.TotalSheepCount += _sheepCount;
        }

        protected override void OnBeDestroyed()
        {
            GameEntry.Event.Unsubscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
        }

        protected override void OnUpdate()
        {
            if (!_start) return;
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval && _sheepCount > 0)
            {
                _timer -= _spawnInterval;
                SpawnSheep();
            }
        }

        private void SpawnSheep()
        {
            _sheepCount--;
            Sheep sheep = Instantiate(_sheepPrefab, transform.position, Quaternion.identity).GetComponent<Sheep>();
        }

        private void OnGameStateChange(object sender, GameEventArgs args)
        {
            var e = (OnGameStateChangeArgs)args;
            if (e.GameState == EGameState.Runtime)
            {
                _start = true;
            }
            else
            {
                _start = false;
            }
        }
    }
}