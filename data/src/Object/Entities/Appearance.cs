using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	/// Appearance contains all of the information about how an Entity is rendered in
	/// the UI (which is a function of the viewer). Name, description, gui, colourscheme, etc.
	public class Appearance : ObjectEntity
	{
		// Apparent type of this object ("Field")
		public virtual string Type				{ get; set; }
		// Apparent detailed description of this object
	    public virtual string Description 		{ get; set; }
		// Apparent brief description of this object
	    public virtual string ShortDescription 	{ get; set; }
	    
	    // This is going to store Other Stuff depending upon what type of entity this is
	    // (for example, icons, colourschemes, etc) - dictionary is (Parameter, Payload)
	    public virtual IDictionary<string, string> Parameters { get; set; }
	
	    // The conditions that must be met in order to "see" this appearance
	    public virtual IList<Condition> Conditions { get; set; }
		
		
		public Appearance()
		{
			this.Parameters	= new Dictionary<string, string>();
			this.Conditions	= new List<Condition>();
		}
		
		
		public bool Valid(IDictionary<string, Trait> traits)
		{
			return this.Conditions.Count(c => traits.ContainsKey(c.Trait) ? c.Valid(traits[c.Trait] as TraitBase) : false) == this.Conditions.Count;
		}
		
		
		public bool Valid(IDictionary<string, Trait> traits, IDictionary<string, Skill> skills)
		{
			return this.Conditions.Count(c => skills.ContainsKey(c.Trait) ? c.Valid(skills[c.Trait] as TraitBase) : (traits.ContainsKey(c.Trait) ? c.Valid(traits[c.Trait] as TraitBase) : false)) == this.Conditions.Count;
		}
	}
}