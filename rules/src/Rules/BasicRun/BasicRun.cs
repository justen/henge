
using System;
using Henge.Rules;
using Henge.Data.Entities;

namespace Henge.Rules.Core
{


	public abstract class BasicRun
	{
		protected string interaction = "Basic Run";
		protected string ruletype = "invalid";
		protected double priority = -1.0;
		public string Interaction
		{
		 	get
			{
				return this.interaction;
			}
			protected set
			{
				this.interaction = value;
			}
		}
		
		public string Ruletype
		{
		 	get
			{
				return this.ruletype;
			}
			protected set
			{
				this.ruletype = value;
			}
		}
		
	
		//this effectively disables all derived rules - override this in specific cases to enable.
		public double Priority (HengeEntity actor)
		{
			this.priority = -1.0;	
			return this.priority;
		}
	}
}
