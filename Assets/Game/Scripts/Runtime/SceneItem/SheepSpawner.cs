using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameMain
{
    public class SheepSpawner : GameEntityBase
    {
        public Transform UITransform;
        [SerializeField] private GameObject _sheepPrefab;
        [SerializeField] private int _sheepCount = 10;
        [SerializeField] private float _spawnInterval = 1;
        [SerializeField] private Color _doorLightBaseColor;
        private bool _start = false;

        private float _timer = 0;
        private int _currentSheepCount;
        private Material _doorLightMaterial;

        protected override void OnInit()
        {
            GameEntry.Event.Subscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
            GameManager.Instance.TotalSheepCount += _sheepCount;
            _doorLightMaterial = transform.Find("Graphics/DOOR/DoorLight").GetComponent<MeshRenderer>().material;
            _currentSheepCount = _sheepCount;
        }

        protected override void OnBeDestroyed()
        {
            GameEntry.Event.Unsubscribe(OnGameStateChangeArgs.EventId, OnGameStateChange);
        }

        protected override void OnUpdate()
        {
            if (!_start) return;
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval && _currentSheepCount > 0)
            {
                _timer -= _spawnInterval;
                SpawnSheep();
            }
        }

        private void SpawnSheep()
        {
            _currentSheepCount--;
            Sheep sheep = Instantiate(_sheepPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity)
                .GetComponent<Sheep>();
            GameEntry.Event.Fire(this, SheepSpawnArgs.Create());
            sheep.gameObject.SetActive(true);
            sheep.AddForce(new Vector3(1, 1).normalized * 10, ForceMode.Impulse);
            var color1 = _doorLightBaseColor * Mathf.Pow(2, 4);
            var color2 = _doorLightBaseColor * Mathf.Pow(2, 2);
            Sequence s = DOTween.Sequence();
            s.Append(_doorLightMaterial.DOColor(color1, "_EmissionColor", 0.2f));
            s.Append(_doorLightMaterial.DOColor(color2, "_EmissionColor", 0.2f));
            s.Play();
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
                _currentSheepCount = _sheepCount;
                _timer = 0;
                GameEntry.Event.Fire(this, SheepSpawnArgs.Create());
            }
        }

        public string GetUIString()
        {
            return $"{_currentSheepCount}/{_sheepCount}";
        }
    }
}