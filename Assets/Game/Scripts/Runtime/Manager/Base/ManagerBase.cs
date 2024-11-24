using System.Threading.Tasks;
using UnityEngine;

namespace GameMain
{
    public abstract class ManagerBase : MonoBehaviour, IManager
    {
        public bool Inited
        {
            get;
            set;
        }
        
        public abstract Task OnEnter();

        public abstract void OnExit();
        public virtual void OnInitEnd()
        {
            
        }
    }
}