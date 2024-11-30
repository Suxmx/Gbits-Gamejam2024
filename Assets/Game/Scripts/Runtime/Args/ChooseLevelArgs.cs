//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-30 15:54:12
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class ChooseLevelArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChooseLevelArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }
        
        public static ChooseLevelArgs Create()
        {
            ChooseLevelArgs args = ReferencePool.Acquire<ChooseLevelArgs>();
            return args;
        }
        
     


        public override void Clear()
        {

        }
    }
}