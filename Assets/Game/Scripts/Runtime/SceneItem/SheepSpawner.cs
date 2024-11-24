using UnityEngine;

namespace GameMain
{
    public class SheepSpawner : GameEntityBase
    {
        [SerializeField] private GameObject _sheepPrefab;
        [SerializeField] private int _sheepCount = 10;
        [SerializeField] private float _spawnInterval = 1;

        private float _timer = 0;
        protected override void OnInit()
        {
                        
        }

        protected override void OnAfterInit()
        {
            
        }

        protected override void OnUpdate()
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnInterval && _sheepCount>0)
            {
                _timer -=_spawnInterval;
                SpawnSheep();
            }
        }

        private void SpawnSheep()
        {
            _sheepCount--;
            Sheep sheep=Instantiate(_sheepPrefab, transform.position, Quaternion.identity).GetComponent<Sheep>();
        }
    }
}