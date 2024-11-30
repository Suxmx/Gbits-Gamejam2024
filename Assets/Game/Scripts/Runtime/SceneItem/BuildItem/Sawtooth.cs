using UnityEngine;

namespace GameMain
{
    public class Sawtooth : GameEntityBase
    {
        private Transform _graphics;
        protected override void OnInit()
        {
            _graphics = transform.Find("Graphics");
        }

        protected override void OnUpdate()
        {
            _graphics.Rotate(Vector3.forward, -360 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Sheep sheep = other.GetComponentInParent<Sheep>();
            if (sheep)
            {
                sheep.Die();
            }
        }
    }
}