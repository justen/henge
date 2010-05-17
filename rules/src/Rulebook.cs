
using System;
using System.Collections.Generic;

namespace Henge.Rules
{
	public class Rulebook
	{
		private Dictionary<string, Ruleset> rules;
		public Rulebook ()
		{
		}
		
		public void Add(IRule rule, string interaction, string role)
		{
			if (this.rules.ContainsKey(interaction)==false) this.rules.Add(interaction, new Ruleset(rule, role));
			else rules[interaction].Add(rule, role);
		}
	}
}
