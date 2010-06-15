using System;


namespace Henge.Data.Entities
{
	public class Trait : ObjectEntity
	{
	    public virtual double Maximum 	{get; set;}
	    public virtual double Minimum 	{get; set;}
	    public virtual double Value		{get; set;}
		public virtual string Flavour	{get; set;}
	
	
		public double Transfer (Trait source, double amount)
		{
			if (this.Value + amount > this.Maximum)
			{
				amount = this.Maximum - this.Value;	
			}
			if (amount>source.Value)
			{
				amount = source.Value;	
			}
			this.Value+=amount;
			return amount;
		}
		
		public double Add (double amount)
		{
			if (amount + this.Value > this.Maximum)
			{
				amount = this.Maximum - this.Value;
				this.Value = this.Maximum;
			}
			else
			{
				this.Value+=amount;
			}
			return amount;
		}
		
		public double Subtract (double amount)
		{
			if (this.Value - amount < this.Minimum)
			{
				amount = this.Minimum + this.Value;
				this.Value = this.Minimum;
			}
			else
			{
				this.Value-=amount;
			}
			return amount;
		}
	}
}