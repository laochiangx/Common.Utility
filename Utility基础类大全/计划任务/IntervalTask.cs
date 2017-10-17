 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;


namespace Common.Utility
{
	/// <summary>
	/// For ASP.Net applications.
	/// 
	/// Starts a thread to perform a task at BelowNormal priority on a regular interval.
	/// 
	/// Meant for long-running ASP.Net apps that never recycle instead of a Windows
	/// Service or cron job.
	/// 
	/// If the task runs longer than the timer interval, the existing thread is left be
	/// and nothing else happens until the next time the interval fires where the worker
	/// thread is not running.
	/// 
	/// Cannot handle multiple tasks; if you have multiple tasks to manage, build on top
	/// of this class by adding task registration to a single task that this class runs.
	/// 
	/// The task is defined as an Action&lt;IntervalTaskContext&gt;, where the argument
	/// it takes is a context object that has a single property: Stopping. If the ASP.Net
	/// app needs to shut down for some reason, and the task is currently running, this
	/// flag will flip to true, and the app will be forcibly torn down by the environment
	/// in 30 seconds. This means the task should check Stopping regularly to see if it
	/// should cut its work short. Note that the 30 seconds is for the entire app to tie
	/// up what it's doing, not just this task - so be conservative.
	/// 
	/// For more on the ASP.Net App teardown process:
	/// http://msdn.microsoft.com/en-us/library/system.web.hosting.iregisteredobject.stop.aspx
	/// http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx
	/// </summary>
	public class IntervalTask : System.Web.Hosting.IRegisteredObject, IDisposable
	{
		/// <summary>
		/// The only instance; set with CreateTask().
		/// </summary>
		public static IntervalTask Current { get; protected set; }

		/// <summary>
		/// Whether the timer is enabled; set with SetInterval() or StopTimer().
		/// </summary>
		public bool Active
		{
			get
			{
				return intervalTimer.Active;
			}
		}

		/// <summary>
		/// Wakeup interval (task may run less frequently than this if it runs long).
		/// </summary>
		public int Interval
		{
			get
			{
				return intervalTimer.Interval;
			}
		}

		/// <summary>
		/// Whether the task is running right now.
		/// </summary>
		public bool TaskRunning { get; protected set; }

		/// <summary>
		/// Stats: The last time SetTimerInterval() was called
		/// </summary>
		public DateTime TimerStarted { get; protected set; }

		/// <summary>
		/// Stats: The last time the timer wokeup; the task may not have started if
		/// an instance was already running.
		/// </summary>
		public DateTime TimerWokeup { get; protected set; }

		/// <summary>
		/// Stats: The last time the task was started
		/// </summary>
		public DateTime TaskStarted { get; protected set; }

		/// <summary>
		/// Stats: The last time the task ended
		/// </summary>
		public DateTime TaskEnded { get; protected set; }

		/// <summary>
		/// Whether the ASP.Net app domain is tearing down.
		/// Tasks should check this property before performing large tasks;
		/// if set to true, the task should cleanup and return to try to
		/// keep cleanup time under 30 seconds. If the task isn't running
		/// when this flag becomes set, the timer will not wake up to start
		/// it again.
		/// </summary>
		public bool ShuttingDown { get; protected set; }

		protected TimerInfo intervalTimer;
		protected Action taskAction;
		protected Thread taskThread;

		private object syncLock = new object();

		protected IntervalTask(Action taskAction)
		{
			System.Web.Hosting.HostingEnvironment.RegisterObject(this);
			this.taskAction = taskAction;
			intervalTimer = new TimerInfo(intervalCallback);
		}

		/// <summary>
		/// Creates a new IntervalTask (and doesn't run it - call SetTimerInterval() to start it).
		/// taskAction format: context => { /* do work */ }
		/// If a task has already been created, throws a FieldAccessException.
		/// </summary>
		/// <param name="taskAction">An Action to be run on an interval</param>
		/// <returns>The created IntervalTask. Call SetTimerInterval() on the returned object to start it.</returns>
		public static IntervalTask CreateTask(Action taskAction)
		{
			if (Current != null)
				throw new FieldAccessException("CreateTask requested, but a task already exists.");

			Current = new IntervalTask(taskAction);
			return Current;
		}

		/// <summary>
		/// If the background task timer is running, changes its interval.
		/// If the timer isn't running, starts the timer (and so, the background task).
		/// </summary>
		/// <param name="interval">The timer interval in milliseconds.</param>
		public void SetInterval(int interval)
		{
			intervalTimer.SetInterval(interval);
		}

		/// <summary>
		/// Stops the timer. If the background task is running when this is called, it's
		/// left be so it can finish it's work, but will not be woken up to start again
		/// until SetTimerInterval() is called with a positive value.
		/// 
		/// Convenience method. This has the same effect as calling
		/// SetTimerInterval(Timeout.Infinite);
		/// </summary>
		public void StopTimer()
		{
			intervalTimer.Stop();
		}

		protected void intervalCallback()
		{
			TimerWokeup = DateTime.UtcNow;

			lock (syncLock)
			{
				// We just woke up. Verify we aren't either still running from a previous wakeup,
				// or stopping because the app is shutting down, before we proceed.
				if (TaskRunning || ShuttingDown)
					return;

				// The task isn't running. Flag that we're now running so the next wakeup won't
				// start a second thread until we unflag.
				TaskRunning = true;
				// TaskRunning is signaled to on here; it has to be signaled to off in the Thread we're creating, in a later method (taskActionWrapper).
			}

			// Track wakeups that actually kick off the task
			TaskStarted = DateTime.UtcNow;

			// Build the thread and run the task action on it at BelowNormal
			taskThread = new Thread(taskActionWrapper);
			taskThread.Priority = ThreadPriority.BelowNormal;
			taskThread.IsBackground = true;
			taskThread.Start();
		}

		protected void taskActionWrapper()
		{
			taskAction();

			TaskEnded = DateTime.UtcNow;

			lock (syncLock)
			{
				TaskRunning = false;

				if (ShuttingDown)
					// Signal this Timer no longer needs to run - let ASP.Net know it can be killed and cleaned up
					System.Web.Hosting.HostingEnvironment.UnregisterObject(this);
			}
		}


		#region IRegisteredObject Members
		/// <summary>
		/// Call if the app is shutting down. Should only be called by the ASP.Net container.
		/// </summary>
		/// <param name="immediate">ASP.Net sets this to false first, then to true the second
		/// call 30 seconds later.</param>
		public void Stop(bool immediate)
		{
			// See: http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx

			lock (syncLock)
			{
				ShuttingDown = true;

				if (!TaskRunning)
					this.Dispose();
			}
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Stops the Timer, and informs the ASP.Net hosting environment that it doesn't need to wait on the IntervalTask to shut down.
		/// </summary>
		public void Dispose()
		{
			StopTimer();
			System.Web.Hosting.HostingEnvironment.UnregisterObject(this);
		}
		#endregion
	}
}
