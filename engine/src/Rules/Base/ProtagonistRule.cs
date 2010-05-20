using System;


namespace Henge.Rules
{
	public class ProtagonistRule : Rule
	{
		public ProtagonistRule ()
		{
		}
		
		
		public override Interaction Apply(Interaction interaction)
		{	
			interaction.Failure("Unable to find protagonist rule", false);
			return interaction;
		}
	}
}
