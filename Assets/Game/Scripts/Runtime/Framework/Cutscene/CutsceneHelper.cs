using System;
using Game.Scripts.Runtime.Cutscene;
using GameMain;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = GameMain.GameEntry;

namespace Game.Scripts.Runtime
{
    public class CutsceneHelper : MonoBehaviour
    {
        public void OnCutsceneEnterEnd()
        {
            GameEntry.Cutscene.AnimEnterEnd();
        }

        public void OnCutsceneFadeEnd()
        {
            GameEntry.Cutscene.AnimFadeEnd();
        }
    }
}