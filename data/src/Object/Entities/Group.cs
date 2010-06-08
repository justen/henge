using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Group : HengeEntity
	{
		public Dictionary<String, GroupRole> Membership	{get; set;}
		public Group()
		{
			this.Membership = new Dictionary<string, GroupRole>();
		}
	}
}
