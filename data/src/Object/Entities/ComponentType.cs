using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class ComponentType : ObjectEntity
	{
		//Identifier to allow us to pull this when creating new Components of a given type
		public string Id	{ get; set; }
		public IDictionary<string, Trait> BaseTraits {get; set;}
		
		public IList<Appearance> Appearance	{ get; set; }
		
		
		public ComponentType()
		{
			this.Appearance = new List<Appearance>();
		}
		
		public ComponentType (Appearance baseAppearance)
		{
			this.Appearance = new List<Appearance>();
			this.Appearance.Add(baseAppearance);
		}
	}
}
