using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Condition : ObjectEntity
	{
		// The attribute this relates to
		public virtual string Trait { get; set; }
		
	    // Minimum value of the attribute to meet the condition
	    public virtual double Minimum { get; set; }
		
	    // Maximum value of the attribute to meet the condition
	    public virtual double Maximum { get; set; }
		
	    // Determine whether to invert the polarity of the result (i.e., you pass if you fail to meet the condition)
	    public virtual bool Invert 	{ get; set; }
		
		
		public bool Valid(TraitBase trait)
		{
			return this.Invert ? (trait.Value < this.Minimum || trait.Value > this.Maximum) : (trait.Value >= this.Minimum && trait.Value <= this.Maximum);
		}	
	}
}