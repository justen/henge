using System;
using System.Collections.Generic;

using Henge.Data.Entities;
using Henge.Rules.Antagonist.Move;

namespace Henge.Rules.Antagonist.Move.Autodetect
{
	public class SwimAntagonist : GenericMovement
	{
	
		public override bool Valid (Component subject)
		{
			return ( (subject is Location) && subject.Traits.ContainsKey("Movement") && subject.Traits["Movement"].Flavour=="Swim");
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
				Location antagonist	= interaction.Antagonist as Location;
				double tariff = antagonist.Traits.ContainsKey("Impede")? antagonist.Traits["Impede"].Value : Constants.Impedance;
				double difficulty = antagonist.Traits["Movement"].Value / Constants.MaxMovementDifficulty;
				double successTariff = tariff - (1-tariff) * (interaction.Protagonist.Skills.ContainsKey("Swim")? interaction.Protagonist.Skills["Swim"].Value : 0);
				
				interaction.Actions.Add( ()=>{
				switch (interaction.ProtagonistCache.SkillCheck("Swim", difficulty+interaction.Difficulty, successTariff+interaction.Impedance, tariff+interaction.Impedance, EnergyType.Strength))
				{
				case SkillResult.PassExhausted:
					interaction.Failure("You feel you are too weak to make it to your destination, and tread water while your strength returns.", false);		
					break;
				case SkillResult.PassSufficient:
					interaction.Log+=("You swim strongly through the water. ");
					break;
				default:
					interaction.Log+= ("You struggle to keep your head above the water. You cough and splutter, panicking, as you realise you can't breathe. ");
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
				});
				if (!(interaction.Protagonist.Location.Traits.ContainsKey("Movement") && interaction.Protagonist.Location.Traits["Movement"].Flavour == "Swim"))
				{
					this.AddTransition(interaction);
				}
				this.AddGuards(interaction);
			return interaction;
		}
	}
}