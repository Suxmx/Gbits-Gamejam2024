//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get { return true; }
        }

        private ProcedureOwner _procedureOwner;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _procedureOwner = procedureOwner;
            GameEntry.Audio.PlayMusic(EMusic.MainMusic, true);
            if (!GameEntry.Procedure.IfPlaySplash)
            {
                DoChangeState();
            }
            else
            {
                GameObject.Find("Splash").GetComponent<Animator>().Play("Splash", 0, 0);
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            procedureOwner.SetData<VarBoolean>("PlayCutscene", false);
        }

        private void DoChangeState()
        {
            ChangeState<ProcedurePreload>(_procedureOwner);
        }

        public void SplashPlayEnd()
        {
            DoChangeState();
        }
    }
}