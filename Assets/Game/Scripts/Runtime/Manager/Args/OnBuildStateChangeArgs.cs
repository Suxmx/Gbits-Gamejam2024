//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-26 14:48:45
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OnBuildStateChangeArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnBuildStateChangeArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }
        
        public EBuildState BuildState { get; private set; }

        public static OnBuildStateChangeArgs Create(EBuildState state)
        {
            OnBuildStateChangeArgs args = ReferencePool.Acquire<OnBuildStateChangeArgs>();
            args.BuildState = state;
            return args;
        }
        
        public override void Clear()
        {

        }
    }
}