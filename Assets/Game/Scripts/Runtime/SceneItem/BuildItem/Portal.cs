using System;
using UnityEngine;

namespace GameMain
{
    public class Portal : BuildItemBase
    {
        public Portal AttachedPortal=>_attachedPortal;
        [SerializeField] private float _radius;
        private bool _enableLogic = false;
        private Portal _attachedPortal;

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
        }

        public override void DisableLogicWhenBuilding()
        {
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
    }
}