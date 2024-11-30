using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class OneWayPlatform : GameEntityBase
    {
        private Collider _collider;
        
        private HashSet<Sheep> _sheepSet = new HashSet<Sheep>();

        private void OnDrawGizmos()
        {
            Gizmos.color=Color.red;
            Gizmos.DrawSphere(transform.position+Vector3.up*0.2f, 0.1f);
        }

        protected override void OnInit()
        {
            _collider = transform.Find("Collider").GetComponent<Collider>();
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
            foreach (var sheep in _sheepSet)
            {
                if (!sheep || sheep.IsDie)
                {
                    continue;
                }
                if (sheep.transform.position.y > transform.position.y+0.2f)
                {
                    Physics.IgnoreCollision(sheep.Collider,_collider,false);
                }
                else
                {
                    Physics.IgnoreCollision(sheep.Collider,_collider,true);
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
    }
}