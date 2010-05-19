using System;
using Henge.Rules;


namespace Henge.Rules.Interference.Move.Run
{
	public class BasicRun : InterferenceRule
	{
		public BasicRun()
		{
		}
		
		
		public override Interaction ContinueInteraction(Interaction interaction)
		{
			return interaction;	
		}
	}
}
