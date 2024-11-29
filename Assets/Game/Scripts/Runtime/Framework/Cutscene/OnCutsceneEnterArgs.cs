//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-29 22:32:55
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OnCutsceneEnterArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnCutsceneEnterArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public static OnCutsceneEnterArgs Create()
        {
            OnCutsceneEnterArgs args = ReferencePool.Acquire<OnCutsceneEnterArgs>();
            return args;
        }


        public override void Clear()
        {

        }
    }
}