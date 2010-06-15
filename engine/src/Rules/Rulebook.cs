using System;
using System.Collections.Generic;


namespace Henge.Rules
{
	public class Rulebook
	{
		private Dictionary<string, Section> rules = new Dictionary<string, Section>();
		
		
		public Rulebook(List<IRule> rules)
		{
			foreach (IRule rule in rules)
			{
				string interaction = rule.Interaction;
				
				if (!this.rules.ContainsKey(interaction)) 	this.rules.Add(interaction, new Section(rule));
				else 										this.rules[interaction].Add(rule);					
			}
		}
		
		
		public Section Section(string interaction)
		{
			Section result = this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
			int trimFrom = interaction.LastIndexOf('.');
			while ((result == null) && (trimFrom > 0))
			{
				interaction = interaction.Remove(trimFrom);
				result = this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
				trimFrom = interaction.LastIndexOf('.');
			}
			return result;
		}
	}
}
