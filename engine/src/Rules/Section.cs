using System;
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
			// Add the default rules
			this.Add(new AntagonistRule());
			this.Add(new ProtagonistRule());
			this.Add(new InterferenceRule());
			
			this.Add(rule);
		}
		
		
		public Interaction ApplyRules(Interaction interaction)
		{
			if (!this.BestRule(this.antagonist, interaction.Antagonist).Apply(interaction).Finished)
			{
				foreach (HengeEntity interferer in interaction.Interferers)
				{
					interaction.Subject = interferer;
					
					if (this.BestRule(this.interference, interferer).Apply(interaction).Finished) break;
				}
				
				if (!interaction.Finished) this.BestRule(this.protagonist, interaction.Protagonist).Apply(interaction);
			}
			
			return interaction;
		}
	
		
		public void Add(IRule rule)
		{
			if (rule is AntagonistRule) 		this.antagonist.Add(rule);
			else if (rule is ProtagonistRule) 	this.protagonist.Add(rule);
			else if (rule is InterferenceRule)	this.interference.Add(rule);
		}
		
		
		private IRule BestRule(List<IRule> rules, HengeEntity subject)
		{
			IRule result		= null;
			double priority 	= -1;
			double maxPriority	= -1;
			
			foreach (IRule candidate in rules)
			{
				priority = candidate.Priority(subject);
				
				if (priority > maxPriority)
				{
					maxPriority	= priority;
					result 		= candidate;
				}
			}
			
			return result;
		}
	}
}
