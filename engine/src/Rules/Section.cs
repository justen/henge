using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class Section
	{
		protected List<IRule> antagonist	= new List<IRule>();
		protected List<IRule> protagonist	= new List<IRule>();
		protected List<IRule> interference	= new List<IRule>();
		
		
		public Section(IRule rule)
		{
			this.Add(rule);
		}
		
		
		public void Merge(Section section)
		{
			this.antagonist.AddRange(section.antagonist);
			this.protagonist.AddRange(section.protagonist);
			this.interference.AddRange(section.interference);
		}
		
		
		public IInteraction ApplyRules(IInteraction interaction)
		{
			IRule rule = this.GetRule(this.antagonist, interaction.Antagonist);
			if (!rule.Apply(interaction).Finished)
			{
				//Now that the AntagonistRule has populated the Interaction with interferers we can work through each of them in turn
				foreach (Component interferer in interaction.Interferers)
				{
					interaction.SetSubject(interferer);
					
					if (this.GetRule(this.interference, interferer).Apply(interaction).Finished) break;
				}
				//...and then apply the final rule and apply the results.
				if (!interaction.Finished) this.GetRule(this.protagonist, interaction.Protagonist).Apply(interaction);
			}
			
			return interaction;
		}
	
		
		public void Add(IRule rule)
		{
			if (rule is IAntagonist) 		this.antagonist.Add(rule);
			else if (rule is IProtagonist) 	this.protagonist.Add(rule);
			else if (rule is IInterferer)	this.interference.Add(rule);
		}
		
		
		private IRule GetRule(List<IRule> rules, Component subject)
		{
			return rules.FirstOrDefault(r => r.Valid(subject));
		}
	}
}
