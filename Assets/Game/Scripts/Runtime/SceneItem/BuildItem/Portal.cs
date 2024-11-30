using System;
using UnityEngine;

namespace GameMain
{
    public class Portal : BuildItemBase
    {
        public Portal AttachedPortal => _attachedPortal;
        [SerializeField] private float _radius;
        private bool _enableLogic = false;
        private Portal _attachedPortal;
        private GameObject _outlineObj;

        public void AttachToPortal(Portal other)
        {
            _attachedPortal = other;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        public override void EnableLogic()
        {
            _enableLogic = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            _enableLogic = false;
        }

        public override bool DetectBuildable()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, _radius, _tmpColliders, _cantBuildLayer,QueryTriggerInteraction.Ignore);
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
            _outlineObj ??= transform.Find("OutlineObject").gameObject;
            _outlineObj.SetActive(bEnable);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_enableLogic || !_attachedPortal) return;
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                sheep.Teleport(_attachedPortal.transform.position);
            }
        }
    }
}