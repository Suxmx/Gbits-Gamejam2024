using System;
using UnityEngine;

namespace GameMain
{
    public class EndPoint : GameEntityBase
    {
        private void OnTriggerEnter(Collider other)
        {
            var sheep = other.GetComponentInParent<Sheep>();
            if (sheep && !sheep.Arrival)
            {
                sheep.Arrival = true;
                Destroy(sheep.gameObject);
                GameManager.Instance.ArriveSheepCount++;
            }
        }
    }
}