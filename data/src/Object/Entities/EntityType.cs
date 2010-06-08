
using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{


	public class EntityType : ObjectEntity
	{

		public string Type								 {get; set;}
		public Appearance BaseAppearance				 {get; set;}
		public List<Appearance> ConditionalAppearance	 {get; set;}
		public EntityType ()
		{
			this.ConditionalAppearance = new List<Appearance>(); 
		}
	}
}
