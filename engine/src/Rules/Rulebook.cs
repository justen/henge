using System;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public class Rulebook
	{
		private Type interactionType = null;
		private SortedDictionary<string, Section> rules = new SortedDictionary<string, Section>();
		
		
		public Rulebook(List<IRule> rules, Type interactionType)
		{
			this.interactionType = interactionType;
			
			foreach (IRule rule in rules)
			{
				string interaction = rule.Interaction;
				
				if (!this.rules.ContainsKey(interaction)) 	this.rules.Add(interaction, new Section(rule));
				else 										this.rules[interaction].Add(rule);					
			}
			
			// Merge sections that descend from other sections. This means that the hierarchy does not
			// need to be negotiated when looking up rules in a section. For example, if you have some
			// generic "Move" rules and then you have a specific "Move.Run" rule section, the "Move.Run"
			// will have all of the rules in "Move" added to it. Since rules are selected in list order
			// the specific "Move.Run" rules will be checked first before moving on to the generic "Move" rules.
			foreach (KeyValuePair<string, Section> kvp in this.rules)
			{
				string interaction 	= kvp.Key;
				int trim 			= interaction.LastIndexOf('.');
				
				if (trim > 0)
				{
					interaction = interaction.Remove(trim);
					if (this.rules.ContainsKey(interaction)) kvp.Value.Merge(this.rules[interaction]);
				}
			}
		}
		
		
		public IInteraction CreateInteraction(DataProvider db, Actor protagonist, Component antagonist, Dictionary<string, object> arguments)
		{
			return (this.interactionType != null) ? (IInteraction)Activator.CreateInstance(this.interactionType, new object [] { db, protagonist, antagonist, arguments }) : null;
		}
		
		
		public Section Section(string interaction)
		{
			Section result = this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
			while (result == null)
			{
				int trim = interaction.LastIndexOf('.');
				
				if (trim > 0)
				{
					interaction = interaction.Remove(trim); 
					result = this.rules.ContainsKey(interaction) ? this.rules[interaction] : null;
				}
				else break;
			} 
			return result;
		}
	}
}
