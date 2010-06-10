using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class ComponentType : ObjectEntity
	{
		public string Id					{ get; set; }
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
