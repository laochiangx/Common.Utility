 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Common.Utility
{
	/// <summary>
	/// A wrapper class for System.Threading.Timer
	/// 
	/// * Eliminates context object to pass in - just use a closure instead.
	/// 
	/// * Allows you to check on the current Interval the timer is set to.
	/// 
	/// * Uses just the one callback per timer - if you need a new callback, make a new TimerInfo.
	/// 
	/// * Simplifies changing schedule of timer.
	/// 
	/// * Easier to check on when it ran (check LastFired).
	/// </summary>
	public class TimerInfo
	{
		/// <summary>
		/// The interval the wrapper Timer is currently set to
		/// </summary>
		public int Interval { get; protected set; }

		/// <summary>
		/// Whether the Timer is currently enabled and running - false if no Timer, or if interval is set to Timeout.Infinite
		/// </summary>
		public bool Active { get; protected set; }

		// Note: This property is thread-safe on 64-bit systems (since DateTimes are 64-bit and therefore atomic). On 32-bit systems, you could read an incomplete value.
		public DateTime LastFired { get; protected set; }

		protected Timer timer;

		protected Action timerCallback;



		/// <summary>
		/// Setup a TimerInfo object.
		/// </summary>
		/// <param name="timerCallback">The Action to call each time the Timer fires. Does away with a context object argument - use a closure instead.</param>
		public TimerInfo(Action timerCallback)
		{
			this.timerCallback = timerCallback;
			Interval = Timeout.Infinite;
			Active = false;
		}


		public void SetInterval(int milliseconds)
		{
			if (timer == null)
			{
				timer = new Timer(new TimerCallback(x =>
				{
					LastFired = DateTime.UtcNow;
					timerCallback();
				}));
			}

			Active = milliseconds != Timeout.Infinite;

			int dueTime = 0;
			if (!Active)
			{
				// If we're stopping the timer, dueTime should play along
				dueTime = Timeout.Infinite;
			}
			else
			{
				// If we're changing the timer interval from one int to another, dueTime should be the remainder of the new Interval
				if (Interval != Timeout.Infinite)
				{
					dueTime = milliseconds - (int)(DateTime.UtcNow - LastFired).TotalMilliseconds;
					if (dueTime < 0)
						dueTime = 0;
				}
				//else
				//{
					// Wasn't even running - leave dueTime at 0
				//}
			}
	
			Interval = milliseconds;


			timer.Change(dueTime, milliseconds);
		}

		public void Stop()
		{
			SetInterval(Timeout.Infinite);
		}
	}
}
