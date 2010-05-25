using System;


namespace Henge.Data.Entities
{


	public class Group : HengeEntity
	{
		public virtual IList<GroupRole> Membership	{get; set;}	
	}
}
