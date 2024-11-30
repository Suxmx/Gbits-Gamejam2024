using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class OneWayPlatform : GameEntityBase
    {
        private Collider _collider;
        
        private HashSet<Sheep> _sheepSet = new HashSet<Sheep>();
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
                if (sheep.transform.position.y > transform.position.y)
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