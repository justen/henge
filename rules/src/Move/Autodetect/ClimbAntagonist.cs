using System;
using System.Collections.Generic;

using Henge.Data.Entities;
using Henge.Rules.Antagonist.Move;

namespace Henge.Rules.Antagonist.Move.Autodetect
{
	public class ClimbAntagonist : GenericMovement
	{
	
		public override bool Valid (Component subject)
		{
			bool result =(subject is Location) && subject.Traits.ContainsKey("Movement") && subject.Traits["Movement"].Flavour=="Climb"; 
			return result;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				
				Location antagonist	= interaction.Antagonist as Location;
				double tariff = antagonist.Traits.ContainsKey("Impede")? antagonist.Traits["Impede"].Value : Constants.Impedance;
				double difficulty = antagonist.Traits["Movement"].Value / Constants.MaxMovementDifficulty;
				double successTariff = tariff - (1-tariff) * (interaction.Protagonist.Skills.ContainsKey("Climb")? interaction.Protagonist.Skills["Climb"].Value : 0);
				
				interaction.Actions.Add( ()=>{
					switch (interaction.ProtagonistCache.SkillCheck("Climb", difficulty+interaction.Difficulty, successTariff+interaction.Impedance, tariff+interaction.Impedance, EnergyType.Strength))
					{
					case SkillResult.PassExhausted:
						interaction.Failure("You can see a likely-looking route over the rocks, but are too tired to follow it. You turn back.", false);		
						break;
					case SkillResult.PassSufficient:
						interaction.Log+=("You climb over the rocks. ");
						break;
					default:
						interaction.Log+=("You slip as you climb across the rocks, falling painfully. ");
						Trait health = interaction.Protagonist.Traits["Health"];
						Trait constitution = interaction.Protagonist.Traits["Constitution"];
						using (interaction.Lock(interaction.Protagonist, health, constitution))
						{
							health.Value -= 3;
							constitution.Value -= 3;
							if (health.Value<=0) 
							{
								constitution.Value = constitution.Minimum;
								health.Flavour = "Dead";
							}
						}
						if (health.Flavour=="Dead")	interaction.Failure("You land badly. The fall is enough to kill you.", false);
						else interaction.Failure("Defeated, you turn back", false);
						break;
					}
				});
	
				this.AddGuards(this.AddTransition(interaction));
			}
			return interaction;
		}
	}
}