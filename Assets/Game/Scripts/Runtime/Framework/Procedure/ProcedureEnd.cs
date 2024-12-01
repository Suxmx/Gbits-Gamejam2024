using GameFramework.Fsm;
using GameFramework.Procedure;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace GameMain
{
    public class ProcedureEnd : ProcedureBase
    {
        public override bool UseNativeDialog => false;


        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            EndAnimHelper helper = Object.FindAnyObjectByType<EndAnimHelper>();
            helper.StartEndAnim();
        }
    }
}