using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Group : Component
	{
		public virtual IDictionary<String, GroupRole> Membership { get; set; }
		
		
		public Group(ComponentType type) : base (type)
		{
			this.Membership = new Dictionary<string, GroupRole>();
		}
		
		public Group()
		{
			this.Membership = new Dictionary<string, GroupRole>();
		}
	}
}
