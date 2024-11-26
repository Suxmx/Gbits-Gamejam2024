using EPOOutline;
using UnityEngine;

namespace GameMain
{
    [RequireComponent(typeof(Outlinable))]
    public abstract class BuildItemBase : GameEntityBase
    {
        protected Rigidbody Rigid
        {
            get
            {
                if (_rigidbody is null)
                    _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody; 
            }
        }
        private Rigidbody _rigidbody;
        private Outlinable _outliner;
        protected Collider[] _tmpColliders = new Collider[10];
        protected LayerMask _cantBuildLayer => LayerMask.GetMask("SheepInteract", "SheepIgnore","CantBuild");
        
        [SerializeField]protected bool _bIsStatic;
        public abstract void EnableLogic();

        public abstract void DisableLogicWhenBuilding();


        public abstract bool DetectBuildable();

        public void SetOutliner(bool bEnable)
        {
            if (_outliner is null)
                _outliner = GetComponent<Outlinable>();
            _outliner.enabled = bEnable;
        }
        public void SetOutlinerColor(Color color)
        {
            color *= Mathf.Pow(2, 1f);
            color.a = 0.6f;
            if (_outliner is null)
                _outliner = GetComponent<Outlinable>();
            _outliner.OutlineParameters.FillPass.SetColor("_PublicColor", color);
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}