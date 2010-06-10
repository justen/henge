using System;


namespace Henge.Data.Entities
{
	public abstract class MapComponent : PhysicalComponent
	{
		public Location Location { get; set; }
	}
}