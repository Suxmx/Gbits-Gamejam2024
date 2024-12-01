//------------------------------------------------------------
// 此文件由工具自动生成
// 生成时间：2024-12-01 10:25:43
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class SheepSpawnArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SheepSpawnArgs).GetHashCode();

        public override int Id
        {
            get { return EventId; }
        }

        public static SheepSpawnArgs Create()
        {
            SheepSpawnArgs args = ReferencePool.Acquire<SheepSpawnArgs>();
            return args;
        }

        public override void Clear()
        {
        }
    }
}