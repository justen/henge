
using System;
using System.Text.RegularExpressions;
using Henge.Data.Entities;

namespace Henge.Rules
{


	public abstract class RuleBase : IRule
	{
		
		private static Regex regex = new Regex(@"Henge\.Rules\.(?:Antagonist|Protagonist|Interference)\.(?<interaction>.+)");
		
		public string Interaction
		{
			get
			{
				Match match = regex.Match(this.GetType().Namespace);
				if (match.Success)
				{
					return match.Result("interaction");	
				}
				else return "undefined";
			}
		}
		
		public double Priority (HengeEntity actor)
		{
			return -1.0;
		}
		
		public RuleBase ()
		{
		
		}
	}
}
