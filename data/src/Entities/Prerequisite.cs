using System;

namespace Henge.Data.Entities
{
	public class Prerequisite : Entity
	{
	    //The attribute that the prerequisite refers to
	    public virtual Attribute Attribute 	{get; set;}
	    //minimum value of the attribute to meet the prerequisite
	    public virtual long Minimum 		{get; set;}
	    //maximum value of the attribute to meet the prerequisite
	    public virtual long Maximum 		{get; set;}
	    //determine whether to invert the polarity of the result (i.e., you pass if you fail
	    //to meet the prerequisite)
	    public virtual bool Invert 			{get; set;}	
	}
}