using System;

using Henge.Data.Entities;
using Henge.Rules.Interference.Move;

namespace Henge.Rules.Interference.Move.Climb
{
	public class EdificeClimbImpede : EdificeImpede
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
			// It's going to impede us significantly less than just trying to barge through it
			// But instead it's going to make the difficulty of the climb check higher
			double climbDifficulty = interaction.Subject.Traits.ContainsKey("Climb")? interaction.Subject.Traits["Climb"].Value : Constants.DefaultClimbDifficulty;
			if (!interaction.Arguments.ContainsKey("Climb"))interaction.Arguments.Add("Climb", climbDifficulty);
			else interaction.Arguments["Climb"] = (double)interaction.Arguments["Climb"]+climbDifficulty;
			interaction.Impedance += interaction.Subject.Traits["Impede"].Value/2.0;
			if (climbDifficulty>1)
			{
				interaction.Log+=string.Format("You are forced to also climb over a {0}", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription);
			}
			else 	interaction.Log+=string.Format("Your climb is made easier by a {0}", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription);
			return interaction;
			
		}
	}
}
