using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class GroupRole : ObjectEntity
	{
		public virtual double Rank								{ get; set; }
		public virtual IList<Avatar> Members 					{ get; set; }
		public virtual IDictionary<string, bool> Permissions	{ get; set; }
		
		
		public GroupRole()
		{
			this.Permissions = new Dictionary<string, bool>();	
		}
	}
}
