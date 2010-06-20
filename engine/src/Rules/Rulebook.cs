using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class Rulebook
	{
		private Type interactionType = null;
		private Dictionary<string, Section> rules = new Dictionary<string, Section>();
		
		
		public Rulebook(List<IRule> rules, Type interactionType)
		{
			this.interactionType = interactionType;
			
			foreach (IRule rule in rules)
			{
				string interaction = rule.Interaction;
				
				if (!this.rules.ContainsKey(interaction)) 	this.rules.Add(interaction, new Section(rule));
				else 										this.rules[interaction].Add(rule);					
			}
		}
		
		
		public IInteraction CreateInteraction(Actor protagonist, Component antagonist)
		{
			return (this.interactionType != null) ? (IInteraction)Activator.CreateInstance(this.interactionType, new object [] { protagonist, antagonist }) : null;
		}
		
		
		public Section Section(string interaction)
		{
			Section result	= this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
			int trimFrom	= interaction.LastIndexOf('.');
			
			while (result == null && trimFrom > 0)
			{
				interaction = interaction.Remove(trimFrom);
				result		= this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
				trimFrom 	= interaction.LastIndexOf('.');
			}
			
			return result;
		}
	}
}
