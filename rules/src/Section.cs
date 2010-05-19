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
			this.Add(new AntagonistRule());
			this.Add(new ProtagonistRule());
			this.Add(new InterferenceRule());
			this.Add(rule);
		}
		
		
		public bool ApplyRule<T>(Interaction interaction, HengeEntity subject) where T : IRule
		{
		   bool result = false;
		   interaction.Subject = subject;
		   
		   if (result is AntagonistRule)    result = this.BestRule(this.antagonist, subject).Apply(interaction);
		   else if (result is ProtagonistRule)  result = this.BestRule(this.protagonist, subject).Apply(interaction);
		   else if (result is InterferenceRule) result = this.BestRule(this.interference, subject).Apply(interaction);
		   
		   return result;

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
