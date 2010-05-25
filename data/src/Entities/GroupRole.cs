using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{



	public class GroupRole : Entity
	{
		public string Name				{get; set;}
		public double Rank				{get; set;}
		public IList<Avatar> Members 	{get; set;}
	}
}
