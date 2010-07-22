using System;


namespace Henge.Data.Entities
{
	public abstract class MapComponent : PhysicalComponent
	{
		public MapComponent(ComponentType type) :base (type)
		{
			
		}
		
		public MapComponent()
		{
			
		}
		
		public Location Location { get; set; }
	}
}