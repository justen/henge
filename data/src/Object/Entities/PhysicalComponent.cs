using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	//Any physical "thing" in the gameworld that isn't a Location
	public abstract class PhysicalComponent : Component
	{
		public PhysicalComponent(ComponentType type) : base(type)
		{
		}
		
		
		public PhysicalComponent()
		{
		}
	}
}