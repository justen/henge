using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class EdificeImpede : HengeRule, IInterferer
	{
		public override double Priority(Component subject)
		{
			return (subject is Edifice) ? 1 : -1;
		}

		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			// Basic impedance rule for any impeding structure - just use the impedance value in "Impede"	
			interaction.Impedance += interaction.Subject.Traits["Impede"].Value;
			
			return interaction;
			
		}
	}
}
