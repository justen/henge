
using System;
using System.Collections.Generic;

namespace Henge.Rules
{


	public class RuleBundle
	{
		private IAntagonistRule antagonist;
		private IProtagonistRule protagonist;
		private IInterferenceRule interference;
		
		public IAntagonistRule Antagonist
		{
			get
			{
				return this.antagonist;
			}
		}
		
		public IProtagonistRule Protagonist
		{
			get
			{
				return this.protagonist;
			}
		}
		
		
		public IInterferenceRule Interference
		{
			get
			{
				return this.interference;
			}
		}
		
		public RuleBundle (IProtagonistRule protagonist, IAntagonistRule antagonist, IInterferenceRule interference)
		{
			this.protagonist = protagonist;
			this.antagonist = antagonist;
			this.interference = interference;
		}
	
	}
}
