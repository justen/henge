using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class GroupRole : ObjectEntity
	{
		public virtual string Name				{get; set;}
		public virtual double Rank				{get; set;}
		public virtual IList<Avatar> Members 	{get; set;}
	}
}
