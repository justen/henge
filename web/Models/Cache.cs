using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Web
{
	public class Cache
	{
		public Dictionary<long, Component> Local 	{ get; set; }
		public Dictionary<long, Component> Remote	{ get; set; }
		
		
		public Cache()
		{
			this.Local	= new Dictionary<long, Component>();
			this.Remote	= new Dictionary<long, Component>();
		}
		
		
		public void ClearAll()
		{
			this.Local.Clear();
			this.Remote.Clear();
		}
		
	}
}
