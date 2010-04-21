
using System;
using System.Collections.Generic;

namespace Henge.Engine.Ruleset
{


	public class RuleBundle
	{
		//dictionary of role->rule 
		private Dictionary <string, IRule> Rules;

		public IAntagonistRule Antagonist
		{
			get
			{
				return this.Rules.ContainsKey("antagonist")?(IAntagonistRule)Rules["antagonist"]:null;
			}
		}
		
		public IProtagonistRule Protagonist
		{
			get
			{
				return this.Rules.ContainsKey("protagonist")?(IProtagonistRule)Rules["protagonist"]:null;
			}
		}
		
		
		public IInterferenceRule Interference
		{
			get
			{
				return this.Rules.ContainsKey("interference")?(IInterferenceRule)Rules["interference"]:null;
			}
		}
		
		public RuleBundle (Dictionary<string, IRule> Rules)
		{
			this.Rules = Rules;
		}
	
	}
}
