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
		}
		
		
		public Trait(double maximum, double minimum, double val)
		{
			this.Maximum	= maximum;
			this.Minimum	= minimum;
			this.Value		= val;
			this.Expiry		= null;
		}
		
		
		public Trait(Trait trait)
		{
			this.Maximum	= trait.Maximum;
			this.Minimum	= trait.Minimum;
			this.Value		= trait.Value;
			this.Flavour	= trait.Flavour;
			this.Subject	= trait.Subject;
			this.Expiry 	= (trait.Expiry == null) ? trait.Expiry : new Nullable<DateTime>(trait.Expiry.Value);
		}
		
		
		public double SetValue(double value)
		{
			base.Value = value > this.Maximum ? this.Maximum : (value < this.Minimum ? this.Minimum : value);
			return base.Value;
		}
	}
}