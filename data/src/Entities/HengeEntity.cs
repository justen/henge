using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class HengeEntity : Entity
	{
	    public virtual EntityType Type					{get; set;}
	    // this appearance is in addition to the Type appearance
	    // the appearance stored in Type is how *all* instances of this
	    // entity appear - these appearances are extra information
	    // in addition to the Type appearance which are specific to this
	    // instance of the entity. For example, the "name" of a Location
	    // may be "House" in the Type appearance, but "29 Acacia Road" in
	    // the specific instance appearance
	    public virtual IList<Appearance> ConditionalAppearance	{get; set;}	
		
		//The generic, you-can-always-see-this appearance of the entity
		public virtual Appearance BaseAppearance {get; set;}
		
		public virtual String Name { get; set; }
		
	    public virtual IList<Statistic> Stats			{get; set;}
	}
}