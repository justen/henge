using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class GroupRole : ObjectEntity
	{
		public double Rank								{ get; set; }
		public IList<Avatar> Members 					{ get; set; }
		public IDictionary<string, bool> Permissions	{ get; set; }
		
		
		public GroupRole()
		{
			this.Permissions = new Dictionary<string, bool>();	
		}
	}
}
