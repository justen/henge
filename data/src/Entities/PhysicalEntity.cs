using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	//Any physical "thing" in the gameworld that isn't a Location
	public class PhysicalEntity : HengeEntity
	{
		//Modifiers currently affecting the entity
		public virtual IList<Modifier> Modifiers {get; set;}
	}
}