using System;
using System.Collections.Generic;

using Henge.Data.Entities;
using Henge.Rules.Protagonist.Move;

namespace Henge.Rules.Protagonist.Move.Autodetect
{
	
	public class SwimProtagonist : MoveRule
	{
		public override bool Valid (Component subject)
		{
			return ( (subject is Actor)&&((Actor)subject).Location.Traits.ContainsKey("Movement") && ((Actor)subject).Location.Traits["Movement"].Flavour=="Swim");
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			return this.Swim(interaction);
		}
		
		protected IInteraction Swim(HengeInteraction interaction)
		{
			if(this.Validate(interaction))
			{
				
				
							Location antagonist	= interaction.Antagonist as Location;
				Actor protagonist = interaction.Protagonist;
				double tariff = 0.25 * (protagonist.Location.Traits.ContainsKey("Impede")? protagonist.Location.Traits["Impede"].Value : Constants.Impedance);
				double difficulty = 0.5 * protagonist.Location.Traits["Movement"].Value / Constants.MaxMovementDifficulty;
				double successTariff = tariff - (1-tariff) * (protagonist.Skills.ContainsKey("Swim")? protagonist.Skills["Swim"].Value : 0);
				switch (interaction.ProtagonistCache.SkillCheck("Swim", difficulty, successTariff, tariff, EnergyType.Fitness))
				{
				case SkillResult.PassExhausted:
					interaction.Log = string.Empty;
					interaction.Failure("You are too weak to continue, and tread water while your strength returns.", false);		
					break;
				case SkillResult.PassSufficient:
					interaction.Log=string.Format("You swim strongly through the water. {0}", interaction.Log);
					break;
				default:
					interaction.Log= ("The currents are too strong for you and pull you back. You cough and splutter, panicking, as you realise you can't breathe. ");
					Trait health = interaction.Protagonist.Traits["Health"];
					Trait constitution = interaction.Protagonist.Traits["Constitution"];
					using (interaction.Lock(interaction.Protagonist, health, constitution))
					{
						health.Value -= 2;
						constitution.Value -= 4;
						if (health.Value<=0) health.Flavour = "Dead";
					}
					if (health.Flavour=="Dead")	interaction.Failure("The light of the surface fades above you. Your struggles subside as your breath runs out. You have died.", false);
					else interaction.Failure("As you flounder, the currents carry you back the way you came", false);
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

