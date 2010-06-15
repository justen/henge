using System;
using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class EdificeImpede : InterferenceRule
	{
		public EdificeImpede()
		{
		}
		
		protected Edifice subject;
		
		public override double Priority (Henge.Data.Entities.Component subject)
		{
			double result = -1;
			if (subject is Edifice)
			{
				this.subject = (Edifice)subject;
				result = 1;
			}
			return result;
		}

		
		public override Interaction Apply(Interaction interaction)
		{
			//basic impedance rule for any impeding structure - just use the impedance value in "impede"
			double impedance = (double)interaction.Transaction["impedance"] + this.subject.Traits["impede"].Value;
			
			interaction.Transaction["impedance"] = impedance;
			return interaction;
			
		}
	}
}
