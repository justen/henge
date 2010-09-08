using System;
using System.Threading;
using System.Collections.Generic;

using Henge.Engine;

namespace Henge.Daemon
{
	public class Heart
	{
		
		private Thread thread = null;
		private bool running = false;
		private bool stop = false;
		public int Sleepiness { get; set; }
		
		public Heart (int sleepiness)
		{
			this.Sleepiness = sleepiness;
		}
		
		public void Start()
		{
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.Heartbeat));
				this.thread.Start();
			}
		}
		
		public void Arrest()
		{
			if (this.thread != null)
			{
				while (this.running)
				{
					this.stop = true;	
				}
			}
		}
		
		private void Heartbeat()
		{
			this.running = true;
			while (this.stop == false)
			{
				System.Threading.Thread.Sleep(this.Sleepiness);
				Henge.Engine.Interactor.Instance.Tick();
			}
			this.running = false;
		}
	}
}

