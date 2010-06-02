using System;


namespace Henge.Data.Entities
{
	public class Statistic : ObjectEntity
	{
	    public virtual Attribute Attribute	{ get; set; }
	    public virtual long Value			{ get; set; }
		
		//public virtual HengeEntity Owner { get; set; }
	}
}