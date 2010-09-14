using System;
using System.Threading;
using System.Collections.Generic;

using Henge.Web;
using Henge.Engine;

namespace Henge.Daemon
{
	public class Heart
	{	
		private Thread thread 	= null;
		private bool running 	= false;
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
				this.running = false;
				while (this.thread.ThreadState != ThreadState.Stopped) Thread.Sleep(10);
				this.thread  = null;
			}
		}
		
		
		private void Heartbeat()
		{
			this.running = true;
			HengeApplication.DataProvider.RegisterContext();
			
			while (this.running)
			{
				Thread.Sleep(this.Sleepiness);
				
				if (Henge.Engine.Interactor.Instance.Tick()) HengeApplication.DataProvider.Flush();
			}
			
			HengeApplication.DataProvider.ReleaseContext();
		}
	}
}

