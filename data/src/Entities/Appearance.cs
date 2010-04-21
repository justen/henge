using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	/// Appearance contains all of the information about how an Entity is rendered in
	/// the UI (which is a function of the viewer). Name, description, gui, colourscheme, etc.
	public class Appearance : Entity
	{
	    //put all the render junk in here. Somehow.
	    public virtual int Priority 			{get; set;}
	    public virtual string Name 				{get; set;}
	    public virtual string Description 		{get; set;}
	    public virtual string ShortDescription 	{get; set;}
	    
	    //this is going to store Other Stuff depending upon what type of entity this is
	    //(for example, icons, colourschemes, etc)
	    public virtual IList<Parameter> Parameters 	{get; set;}
	
	    //The conditions that must be met in order to "see" this appearance
	    public virtual IList<Prerequisite> Prerequisites 		{get; set;}
	}
}