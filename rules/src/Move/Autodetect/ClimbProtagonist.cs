using System;
using System.Collections.Generic;

using Henge.Data.Entities;
using Henge.Rules.Protagonist.Move;

namespace Henge.Rules.Protagonist.Move.Autodetect
{
	public class ClimbProtagonist : MoveRule
	{
		public override bool Valid (Component subject)
		{
			return ( (subject is Actor)&&((Actor)subject).Location.Traits.ContainsKey("Movement") && ((Actor)subject).Location.Traits["Movement"].Flavour=="Climb");
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			return this.Climb(interaction);
		}
		
		protected IInteraction Climb(HengeInteraction interaction)
		{
			if(this.Validate(interaction))
			{

				Location antagonist	= interaction.Antagonist as Location;
				Actor protagonist = interaction.Protagonist;
				double tariff = 0.25 * (protagonist.Location.Traits.ContainsKey("Impede")? protagonist.Location.Traits["Impede"].Value : Constants.Impedance);
				double difficulty = 0.5 * protagonist.Location.Traits["Movement"].Value / Constants.MaxMovementDifficulty;
				double successTariff = tariff - (1-tariff) * (protagonist.Skills.ContainsKey("Climb")? protagonist.Skills["Climb"].Value : 0);
				switch (interaction.ProtagonistCache.SkillCheck("Climb", difficulty, successTariff, tariff, EnergyType.Strength))
					{
					case SkillResult.PassExhausted:
						interaction.Log = string.Empty;
						interaction.Failure("You feel too weak to set out; you pause to gather your strength.", false);		
						break;
					case SkillResult.PassSufficient:
						interaction.Log=string.Format("You climb across the {0}. {1}", interaction.Protagonist.Location.Inspect(interaction.Protagonist).ShortDescription, interaction.Log);
					
						break;
					default:
						interaction.Log = string.Empty;
						interaction.Failure("The terrain is too difficult for you to climb through. You appear to be trapped", false);
						break;
					}
		
				if (!interaction.Finished)
				{
					foreach (Action action in interaction.Actions)
					{
						if (interaction.Finished) break;
						else action.Invoke();
					}
				}
				if (!interaction.Finished)
				{
					this.ApplyInteraction(interaction, interaction.Protagonist, antagonist);	
				}
			}
			return interaction;
		}
	}
}

