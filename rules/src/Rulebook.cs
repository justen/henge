
using System;
using System.Collections.Generic;

namespace Henge.Rules
{
	public class Rulebook
	{
		private Dictionary<string, Ruleset> rules = new Dictionary<string, Ruleset>();
		public Rulebook (List<IRule> rules)
		{
			if (rules != null)
			{
				foreach (IRule rule in rules)
				{
					string interaction = rule.Interaction;
					if (this.rules.ContainsKey(interaction)==false) this.rules.Add(interaction, new Ruleset(rule));
					else this.rules[interaction].Add(rule);					
				}
			}
		}
	}
}
