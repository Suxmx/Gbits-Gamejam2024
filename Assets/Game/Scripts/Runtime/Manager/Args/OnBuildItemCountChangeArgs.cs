//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-12-01 06:09:54
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class OnBuildItemCountChangeArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(OnBuildItemCountChangeArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }
        
        public EBuildItem BuildItem { get; private set; }
        public int Count { get; private set; }

        public static OnBuildItemCountChangeArgs Create(EBuildItem item,int count)
        {
            OnBuildItemCountChangeArgs args = ReferencePool.Acquire<OnBuildItemCountChangeArgs>();
            args.Count = count;
            args.BuildItem = item;
            return args;
        }


        public override void Clear()
        {

        }
    }
}