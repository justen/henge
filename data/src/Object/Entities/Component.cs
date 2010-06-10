using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Component : ObjectEntity
	{
		public String Name 								{ get; set; }
		public ComponentType Type						{ get; set; }
	    public IDictionary<string, long> Attributes 	{ get; set; }

		// String containing the detail of this instances appearance
		public string Detail { get; set; }
		
		
		public Component()
		{
			this.Attributes = new Dictionary<string, long>();
		}
		
		
		public Appearance Appearance()
		{
			return this.Type.Appearance.FirstOrDefault();
		}
		
		
		public Appearance Inspect(Component inspector)
		{
			return this.Type.Appearance.LastOrDefault(a => a.Valid(inspector.Attributes));
		}
	}
}