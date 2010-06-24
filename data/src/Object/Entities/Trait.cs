using System;


namespace Henge.Data.Entities
{
	public class Trait : ObjectEntity
	{
	    public virtual double Maximum 				{get; set;}
	    public virtual double Minimum 				{get; set;}
	    public virtual double Value					{get; set;}
		public virtual string Flavour				{get; set;}
		public virtual Nullable<DateTime> Expiry	{get; set;}
		public virtual Component Subject			{get; set;}
		
		public Trait()
		{
			this.Expiry = null;
		}
	}
}