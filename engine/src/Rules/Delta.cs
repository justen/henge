using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	/*public class Delta
	{
		public Component Component { get; set; }
		
		public IList<Func<Component, bool, bool>> Deltas { get; set; }
		
		
		public Delta(Component component)
		{
			this.Component 	= component;
			this.Deltas		= new List<Func<Component, bool, bool>>();
		}
		
		
		public bool ApplyDeltas(bool success)
		{
			foreach (Func<Component, bool, bool> delta in this.Deltas)
			{
				if (!delta(this.Component, success)) return false;
			}
			
			return true;
		}
	}*/
}
