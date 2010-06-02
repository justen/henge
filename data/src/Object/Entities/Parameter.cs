using System;

namespace Henge.Data.Entities
{
	/// Describes nonstandard appearance attributes (i.e., attributes that not all
	/// entities have
	public class Parameter : ObjectEntity
	{
	    public virtual string Name		{get; set;}
	    public virtual string Setting	{get; set;}
	}
}