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
		public string Type				{ get; set; }
		// Apparent detailed description of this object
	    public string Description 		{ get; set; }
		// Apparent brief description of this object
	    public string ShortDescription 	{ get; set; }
	    
	    // This is going to store Other Stuff depending upon what type of entity this is
	    // (for example, icons, colourschemes, etc) - dictionary is (Parameter, Payload)
	    public IDictionary<string, string> Parameters { get; set; }
	
	    // The conditions that must be met in order to "see" this appearance
	    public IList<Condition> Conditions { get; set; }
		
		
		public Appearance()
		{
			this.Parameters	= new Dictionary<string, string>();
			this.Conditions	= new List<Condition>();
		}
		
		
		public bool Valid(IDictionary<string, Trait> traits)
		{
			return this.Conditions.Count(c => traits.ContainsKey(c.Trait) ? c.Valid(traits[c.Trait]) : false) == this.Conditions.Count;
		}
	}
}