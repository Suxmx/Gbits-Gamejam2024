//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-26 14:57:08
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OnPauseStateChangeArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnPauseStateChangeArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public static OnPauseStateChangeArgs Create(bool pause)
        {
            OnPauseStateChangeArgs args = ReferencePool.Acquire<OnPauseStateChangeArgs>();
            args.Pause = pause;
            return args;
        }
        public bool Pause { get; private set; }

        public override void Clear()
        {

        }
    }
}