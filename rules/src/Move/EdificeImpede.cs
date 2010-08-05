using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class EdificeImpede : HengeRule, IInterferer
	{
		public override bool Valid(Component subject)
		{
			return subject is Edifice;
		}

		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Do not change visibility
			subject = null;
			return -1;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			// Basic impedance rule for any impeding structure - just use the impedance value in "Impede"
			if (this.Validate(interaction))
			{

				interaction.Impedance += interaction.Subject.Traits["Impede"].Value;
				interaction.Log+=string.Format("A {0} hinders your progress", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription);

			}
			return interaction;
			
		}
	}
}
