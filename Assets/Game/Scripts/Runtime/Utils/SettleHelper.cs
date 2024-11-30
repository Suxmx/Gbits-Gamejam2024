using UnityEngine;

namespace GameMain
{
    public class SettleHelper : MonoBehaviour
    {
        public void OnAnimEnd()
        {
            GameEntry.UI.CloseUIForm(UIFormId.SettleUIForm);
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain)?.NextLevel();
        }
    }
}