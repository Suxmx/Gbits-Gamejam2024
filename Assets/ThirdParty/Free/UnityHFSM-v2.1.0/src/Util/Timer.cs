using UnityEngine;

namespace UnityHFSM
{
	/// <summary>
	/// Default timer that calculates the elapsed time based on Time.time.
	/// </summary>
	public class Timer : ITimer
	{
		public float startTime;
		public virtual float Elapsed => Time.time - startTime;

		public virtual void Reset()
		{
			startTime = Time.time;
		}

		public static bool operator >(Timer timer, float duration)
			=> timer.Elapsed > duration;

		public static bool operator <(Timer timer, float duration)
			=> timer.Elapsed < duration;

		public static bool operator >=(Timer timer, float duration)
			=> timer.Elapsed >= duration;

		public static bool operator <=(Timer timer, float duration)
			=> timer.Elapsed <= duration;
	}
}
