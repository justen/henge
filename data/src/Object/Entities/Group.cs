using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Group : Component
	{
		public IDictionary<String, GroupRole> Membership { get; set; }
		
		
		public Group()
		{
			this.Membership = new Dictionary<string, GroupRole>();
		}
	}
}
