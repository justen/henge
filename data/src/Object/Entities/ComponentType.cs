using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class ComponentType : ObjectEntity
	{
		//Identifier to allow us to pull this when creating new Components of a given type
		public virtual string Id								{ get; set; }
		public virtual IDictionary<string, Trait> BaseTraits	{ get; set; }
		public virtual IDictionary<string, Skill> BaseSkills	{ get; set; }
		public virtual IList <Tick>  BaseTick					{ get; set; }
		public virtual IList<Appearance> Appearance				{ get; set; }
		
		
		public ComponentType()
		{
			this.Appearance = new List<Appearance>();
			this.BaseTraits = new Dictionary< string, Trait>();
			this.BaseSkills = new Dictionary<string, Skill>();
			this.BaseTick = new List<Tick>();
		}
		
		
		public ComponentType(Appearance baseAppearance)
		{
			this.Appearance = new List<Appearance>();
			this.BaseTraits = new Dictionary< string, Trait>();
			this.BaseSkills = new Dictionary<string, Skill>();
			this.BaseTick = new List<Tick>();
			this.Appearance.Add(baseAppearance);
		}
	}
}
