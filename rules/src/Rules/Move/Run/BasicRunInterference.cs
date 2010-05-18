using System;
using Henge.Rules;


namespace Henge.Rules.Interference.Move.Run
{
	public class BasicRun : Rule, IInterferenceRule
	{
		public BasicRun()
		{
		}
		
		
		public Interaction ContinueInteraction(Interaction interaction)
		{
			return interaction;	
		}
	}
}
