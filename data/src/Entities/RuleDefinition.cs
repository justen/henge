using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class RuleDefinition : Entity
	{
	    //Interaction mode we want a rule for (for example, "Search", "Attack")
	    public virtual string Interaction	{get; set;}	
	    //Name of the rule to apply
	    public virtual string RuleName		{get; set;}
		//Rule parameters
		public virtual IList<Parameter> Parameters {get; set;}
	}
}