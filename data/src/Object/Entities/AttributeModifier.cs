using System;


namespace Henge.Data.Entities
{
	public class AttributeModifier : ObjectEntity
	{
		public virtual Attribute Target 	{get; set;}
		public virtual long Modification	{get; set;}

	}
}
