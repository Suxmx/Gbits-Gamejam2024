using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class OneWayPlatform : BuildItemBase
    {
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private Vector3 _offset;

        private Collider _collider;
        private Collider _trigger;
        private bool _enableLogic = true;

        private HashSet<Sheep> _sheepSet = new HashSet<Sheep>();

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + _offset, _boxSize);
        }


        private void OnTriggerEnter(Collider other)
        {
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                _sheepSet.Add(sheep);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_enableLogic) return;
            foreach (var sheep in _sheepSet)
            {
                if (!sheep || sheep.IsDie)
                {
                    continue;
                }

                if (sheep.transform.position.y > transform.position.y )
                {
                    Physics.IgnoreCollision(sheep.Collider, _collider, false);
                }
                else
                {
                    Physics.IgnoreCollision(sheep.Collider, _collider, true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                _sheepSet.Remove(sheep);
            }
        }

        public override void EnableLogic()
        {
            _enableLogic = true;
            _collider ??= transform.Find("Collider").GetComponent<Collider>();
            _trigger ??= transform.Find("Trigger").GetComponent<Collider>();

            _trigger.enabled = true;
            _collider.enabled = true;
        }

        public override void DisableLogicWhenBuilding()
        {
            _enableLogic = false;
            _sheepSet.Clear();
            _trigger ??= transform.Find("Trigger").GetComponent<Collider>();
            _collider ??= transform.Find("Collider").GetComponent<Collider>();
            _trigger.enabled = false;
            _collider.enabled = false;
        }

        public override bool DetectBuildable()
        {
            var size = Physics.OverlapBoxNonAlloc(transform.position + _offset, _boxSize / 2, _tmpColliders,
                transform.rotation, _cantBuildLayer, QueryTriggerInteraction.Ignore);
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