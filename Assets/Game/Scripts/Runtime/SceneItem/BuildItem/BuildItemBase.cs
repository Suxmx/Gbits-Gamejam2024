using UnityEngine;

namespace GameMain
{
    public abstract class BuildItemBase : GameEntityBase
    {
        [SerializeField]protected bool _bIsStatic;
        public abstract void EnableLogic();

        public abstract void DisableLogicWhenBuilding();
    }
}