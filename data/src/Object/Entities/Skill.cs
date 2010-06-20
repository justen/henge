using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Skill
	{
		public double Value 			{get; set;}
		public IList<string> Children	{get; set;}
		
		
		public Skill()
		{
			this.Children = new List<string>();	
		}
		
		
		public double Subtract(double quantity)
		{
			this.Value -= quantity;
			if (this.Value < 0.0) this.Value = 0.0;
			
			return this.Value;
		}	
		
		
		public double Add(double quantity)
		{
			this.Value += quantity;
			if (this.Value > 1.0) this.Value = 1.0;
			
			return this.Value;			
		}
	}
}
