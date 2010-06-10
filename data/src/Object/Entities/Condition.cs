using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Condition : ObjectEntity
	{
		// The attribute this relates to
		public string Attribute { get; set; }
		
	    // Minimum value of the attribute to meet the condition
	    public long Minimum { get; set; }
		
	    // Maximum value of the attribute to meet the condition
	    public long Maximum { get; set; }
		
	    // Determine whether to invert the polarity of the result (i.e., you pass if you fail to meet the condition)
	    public bool Invert 	{ get; set; }
		
		
		public bool Valid(long attribute)
		{
			return this.Invert ? (attribute < this.Minimum || attribute > this.Maximum) : (attribute >= this.Minimum && attribute <= this.Maximum);
		}		
	}
}