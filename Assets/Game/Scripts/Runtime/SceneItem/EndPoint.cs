using System;
using DG.Tweening;
using UnityEngine;

namespace GameMain
{
    public class EndPoint : GameEntityBase
    {
        public Transform UITransform;
        [SerializeField] private Color _doorLightBaseColor;
        private Material _doorLightMaterial;

        protected override void OnInit()
        {
            _doorLightMaterial = transform.Find("Graphics/DOOR/DoorLight").GetComponent<MeshRenderer>().material;
        }

        private void OnTriggerEnter(Collider other)
        {
            var sheep = other.GetComponentInParent<Sheep>();
            if (sheep && !sheep.Arrival)
            {
                sheep.Arrival = true;
                Destroy(sheep.gameObject);
                GameManager.Instance.ArriveSheepCount++;
                OnSheepArrival();
            }
        }

        private void OnSheepArrival()
        {
            var color1 = _doorLightBaseColor * Mathf.Pow(2, 4);
            var color2 = _doorLightBaseColor * Mathf.Pow(2, 2);
            Sequence s = DOTween.Sequence();
            s.Append(_doorLightMaterial.DOColor(color1, "_EmissionColor", 0.2f));
            s.Append(_doorLightMaterial.DOColor(color2, "_EmissionColor", 0.2f));
            s.Play();
        }

        public string GetUIString()
        {
            return $"{GameManager.Instance.ArriveSheepCount}/{GameManager.Instance.TotalSheepCount}";
        }
    }
}