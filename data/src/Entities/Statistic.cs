using System;

namespace Henge.Data.Entities
{
	public class Statistic : Entity
	{
	    public virtual Attribute Attribute	{get; set;}
	    public virtual long Value		{get; set;}
	}
}