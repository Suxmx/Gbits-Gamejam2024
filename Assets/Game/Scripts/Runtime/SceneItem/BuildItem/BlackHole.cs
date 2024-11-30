using System;
using UnityEngine;

namespace GameMain
{
    public class BlackHole : BuildItemBase
    {
        [SerializeField] private float _radius;
        
        private bool _bEnableLogic = false;
        private GameObject _outlineObject;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
        public override void EnableLogic()
        {
            _bEnableLogic = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            _bEnableLogic = false;
        }

        public override bool DetectBuildable()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, _radius, _tmpColliders, _cantBuildLayer);
            for (int i = 0; i < Mathf.Min(size, 10); i++)
            {
                if (_tmpColliders[i].transform != transform && !_tmpColliders[i].transform.IsChildOf(transform))
                {
                    return false;
                }
            }

            return true;
        }

        public override void SetOutliner(bool bEnable)
        {
            base.SetOutliner(bEnable);
            _outlineObject ??= transform.Find("OutlineObject").gameObject;
            _outlineObject.SetActive(bEnable);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_bEnableLogic) return;
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                var dist = Vector2.Distance(transform.position, sheep.transform.position);
                if (dist < 0.5f)
                {
                    sheep.Die();
                }
                else
                {
                    var direction = (transform.position - sheep.transform.position).normalized;
                    sheep.AddForce(direction * 10 / (dist * dist), ForceMode.Acceleration);
                }
            }
        }
    }
}