using System;


namespace Henge.Data.Entities
{
	public class Trait : TraitBase
	{
	    public virtual double Maximum 				{get; set;}
	    public virtual double Minimum 				{get; set;}
		public virtual string Flavour				{get; set;}
		public virtual Nullable<DateTime> Expiry	{get; set;}
		public virtual Component Subject			{get; set;}
		
		public Trait()
		{
			this.Expiry = null;
		}
		
		
		public override double Value {
			get {
				return base.Value;
			}
			set {
				base.Value = (value>this.Maximum)? this.Maximum : ((value<this.Minimum)? this.Minimum : value);
			}
		}
	}
}