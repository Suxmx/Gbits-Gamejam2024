using System;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

namespace GameMain
{
    public class Pendulum : BuildItemBase
    {
        [SerializeField] private float _radius;
        private bool _enableLogic = false;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        public override void EnableLogic()
        {
            _enableLogic = true;
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                col.enabled = true;
            }
        }

        public override void DisableLogicWhenBuilding()
        {
            _enableLogic = false;
            foreach (var col in GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
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

        protected override void OnFixedUpdate()
        {
            if (_enableLogic)
            {
                Quaternion deltaRotation = Quaternion.Euler(Vector3.forward * (360 * 0.5f * Time.fixedDeltaTime));

                // 应用旋转
                Rigid.MoveRotation(Rigid.rotation * deltaRotation);
            }
        }
    }
}