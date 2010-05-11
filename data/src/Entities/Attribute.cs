using System;


namespace Henge.Data.Entities
{
	public class Attribute : Entity
	{
	    public virtual string Name 		{get; set;}
	    public virtual long Maximum 	{get; set;}
	    public virtual long Minimum 	{get; set;}
	    public virtual long Step 		{get; set;}
	}
}