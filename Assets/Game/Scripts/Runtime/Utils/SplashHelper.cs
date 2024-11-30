using UnityEngine;

namespace GameMain
{
    public class SplashHelper : MonoBehaviour
    {
        public void OnSplashPlayEnd()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureSplash).SplashPlayEnd();
        }
}
}