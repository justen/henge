using System;
using System.Text.RegularExpressions;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public abstract class Rule : IRule
	{
		private static Regex regex = new Regex(@"Henge\.Rules\.(?:Antagonist|Protagonist|Interference)\.(?<interaction>.+)");
		
		public string Interaction
		{
			get
			{
				Match match = regex.Match(this.GetType().Namespace);
				
				return match.Success ? match.Result("interaction") : "Undefined";
			}
		}
		
		
		public virtual double Priority(HengeEntity actor)
		{
			return 0;
		}
		
		public virtual bool Apply(Interaction interaction)
		{
			return true;	
		}
		
		public Rule()
		{
		
		}
	}
}
