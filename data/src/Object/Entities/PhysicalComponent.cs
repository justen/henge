using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	//Any physical "thing" in the gameworld that isn't a Location
	public abstract class PhysicalComponent : Component
	{
		//Modifiers currently affecting the entity
		// Dictionary is <ModifierName, ModifierData>
		public IDictionary<string, Modifier> Modifiers { get; set; }
	}
}