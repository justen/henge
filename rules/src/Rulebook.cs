using System;
using System.Collections.Generic;


namespace Henge.Rules
{
	public class Rulebook
	{
		private Dictionary<string, Set> rules = new Dictionary<string, Set>();
		
		public Rulebook (List<IRule> rules)
		{
			if (rules != null)
			{
				foreach (IRule rule in rules)
				{
					string interaction = rule.Interaction;
					
					if (!this.rules.ContainsKey(interaction)) 	this.rules.Add(interaction, new Set(rule));
					else 										this.rules[interaction].Add(rule);					
				}
			}
		}
		
		public Set Chapter(string interaction)
		{
			return rules.ContainsKey(interaction) ? rules[interaction] : null;
		}
	}
}
