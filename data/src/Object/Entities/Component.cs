using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Component : ObjectEntity
	{
		public String Name 								{ get; set; }
		public ComponentType Type						{ get; set; }
	    public IDictionary<string, Trait> Traits 		{ get; set; }
		public DateTime LastModified					{ get; set; }
		public DateTime Created							{ get; set; }
		public IList<Item> Inventory			  		{ get; set; }
		// String containing the detail of this instances appearance
		public string Detail { get; set; }
		
		
		public Component()
		{
			this.Inventory	= new List<Item>();
			this.Created = DateTime.Now;
			this.LastModified = DateTime.Now;
			this.Traits = new Dictionary<string, Trait>();
		}
		
		
		public Appearance Appearance()
		{
			return this.Type.Appearance.FirstOrDefault();
		}
		
		
		public Appearance Inspect(Component inspector)
		{
			if (inspector is Actor) return this.Type.Appearance.LastOrDefault(a => a.Valid(inspector.Traits, (inspector as Actor).Skills));
			else return this.Type.Appearance.LastOrDefault(a => a.Valid(inspector.Traits));
		}
	}
}