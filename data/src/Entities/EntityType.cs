using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class EntityType : Entity
	{
	    // Name of the location type (this is the actual, real name of the entity type)
	    public virtual string RealName {get; set;}
	    
		//The generic, you-can-always-see-this appearance of the entity type
		public virtual Appearance BaseAppearance {get; set;}
		
	    // Set of Appearance objects - these are used to figure out how to render the entity based
	    // upon the observer 
	    public virtual IList<Appearance> ConditionalAppearances {get; set;}
	
	    // Set of Attributes objects - describe the numerical attributes (and ranges thereof) available to this entity type
		// These are the BASE attributes. Skills and modifiers may add more to instances of this Type.
		// many2many
	    public virtual IList<Attribute> Attributes {get; set;}

		// Set of Attributes objects - describe the numerical attributes (and ranges thereof) available to this entity type
		// These are the BASE skills that the Type always has available. Skills and modifiers may add more to instances of this Type.
		// many2many
		public virtual IList<Skill> Skills {get; set;}
		
	}
}