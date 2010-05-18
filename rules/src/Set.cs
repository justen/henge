using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class Set
	{
		protected List<IRule> antagonist	= new List<IRule>();
		protected List<IRule> protagonist	= new List<IRule>();
		protected List<IRule> interference	= new List<IRule>();
		
		
		public Set(IRule rule)
		{
			this.Add(rule);
		}
		
		
		public T Rule<T>(HengeEntity subject) where T : IRule
		{
			T result = default(T);
			
			if (result is IAntagonistRule) 			result = (T)this.BestRule(this.antagonist, subject);
			else if (result is IProtagonistRule) 	result = (T)this.BestRule(this.protagonist, subject);
			else if (result is IInterferenceRule)	result = (T)this.BestRule(this.interference, subject);
			
			return result;
		}
	
		
		public void Add(IRule rule)
		{
			if (rule is IAntagonistRule) 		this.antagonist.Add(rule);
			else if (rule is IProtagonistRule) 	this.protagonist.Add(rule);
			else if (rule is IInterferenceRule)	this.interference.Add(rule);
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
