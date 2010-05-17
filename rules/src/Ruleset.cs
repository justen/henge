
using System;
using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Rules
{


	public class Ruleset
	{
		protected List<IRule> antagonist = new List<IRule>();
		protected List<IRule> protagonist = new List<IRule>();
		protected List<IRule> interference = new List<IRule>();
		
		public IAntagonistRule Antagonist(HengeEntity antagonist)
		{
			IAntagonistRule result = null;
			if ((this.antagonist!=null) && (this.antagonist.Count>0))
			{
				result = (IAntagonistRule)this.BestRule(this.antagonist, antagonist);	
			}
			return result;
		}
		
		public IProtagonistRule Protagonist(Actor protagonist)
		{
			IProtagonistRule result = null;
			if ((this.protagonist!=null) && (this.protagonist.Count>0))
			{
				result = (IProtagonistRule)this.BestRule(this.protagonist, (HengeEntity)protagonist);	
			}
			return result;
		}
		
		public IInterferenceRule Interference(HengeEntity interferer)
		{
			IInterferenceRule result = null;
			if ((this.interference!=null) && (this.interference.Count>0))
			{
				result = (IInterferenceRule)this.BestRule(this.interference, interferer);	
			}
			return result;
		}
		
		public Ruleset(IRule rule, string role)
		{
			this.Add(rule, role);
		}
			
		public void Add(IRule rule, string role)
		{
			if (role=="antagonist") this.antagonist.Add((IAntagonistRule)rule);
			if (role=="protagonist") this.protagonist.Add((IProtagonistRule)rule);
			if (role=="interference") this.interference.Add((IInterferenceRule)rule);
		}
		
		protected IRule BestRule(List<IRule> rules, HengeEntity entity)
		{
			IRule result = null;
			double priority = -1;
			double newPriority = -1;
			foreach (IRule candidate in rules)
			{
				if (candidate.EvaluatePriority(entity) > priority)
				{
					priority = candidate.Priority;
					result = candidate;
				}
			}
			return result;
		}
	}
}
