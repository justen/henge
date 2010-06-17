using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	/*public class EdificeImpede : InterferenceRule
	{
		public override double Priority(Component subject)
		{
			return (subject is Edifice) ? 1 : -1;
		}

		
		public override Interaction Apply(Interaction interaction)
		{
			//basic impedance rule for any impeding structure - just use the impedance value in "impede"
			double impedance = (double)interaction.Transaction["impedance"] + interaction.Subject.Traits["impede"].Value;
			
			interaction.Transaction["impedance"] = impedance;
			return interaction;
			
		}
	}*/
}
