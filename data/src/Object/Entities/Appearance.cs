using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	/// Appearance contains all of the information about how an Entity is rendered in
	/// the UI (which is a function of the viewer). Name, description, gui, colourscheme, etc.
	public class Appearance : ObjectEntity
	{
		public Appearance()
		{
			this.Parameters = new Dictionary<string, string>();
			this.Prerequisites = new Dictionary<string, Prerequisite>();
		}
		
	    //put all the render junk in here. Somehow.
	    public int Priority 			{get; set;}
		// apparent name of this object ("USS Enterprise")
	    public string Name 				{get; set;}
		// apparent type of this object ("Starship")
		public string Type				{get; set;}
		// apparent detailed description of this object
	    public string Description 		{get; set;}
		// apparent brief description of this object
	    public string ShortDescription 	{get; set;}
	    
	    //this is going to store Other Stuff depending upon what type of entity this is
	    //(for example, icons, colourschemes, etc) - dictionary is (Parameter, payload)
	    public Dictionary<string, string> Parameters 	{get; set;}
	
	    //The conditions that must be met in order to "see" this appearance Dictionary is (AttributeName, Prerequisite)
	    public Dictionary<string, Prerequisite> Prerequisites 		{get; set;}
	}
}