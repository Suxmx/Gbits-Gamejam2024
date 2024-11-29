//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-11-30 02:28:34
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SheepArriveArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SheepArriveArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public static SheepArriveArgs Create(int arrive,int total)
        {
            SheepArriveArgs args = ReferencePool.Acquire<SheepArriveArgs>();
            args.TotalSheepCount = total;
            args.ArriveSheepCount = arrive;
            return args;
        }

        public int TotalSheepCount { get; set; }
        public int ArriveSheepCount { get; set; }


        public override void Clear()
        {
            TotalSheepCount = 0;
            ArriveSheepCount = 0;
        }
    }
}