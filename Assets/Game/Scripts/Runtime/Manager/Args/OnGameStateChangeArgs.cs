//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-26 15:54:32
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OnGameStateChangeArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnGameStateChangeArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public EGameState GameState { get; private set; }

        public static OnGameStateChangeArgs Create(EGameState state)
        {
            OnGameStateChangeArgs args = ReferencePool.Acquire<OnGameStateChangeArgs>();
            args.GameState = state;
            return args;
        }


        public override void Clear()
        {
        }
    }
}