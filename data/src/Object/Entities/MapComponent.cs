using System;


namespace Henge.Data.Entities
{
	public abstract class MapComponent : PhysicalComponent
	{
		public virtual Location Location { get; set; }
		
		
		public MapComponent(ComponentType type) : base(type)
		{
		}
		
		
		public MapComponent()
		{	
		}
	}
}