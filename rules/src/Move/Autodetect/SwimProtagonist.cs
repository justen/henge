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
			if(! interaction.Finished)
			{
				Actor protagonist = interaction.Protagonist;
				Location antagonist	= interaction.Antagonist as Location;
				if (interaction.Protagonist != null && antagonist != null)
				{
					
					Location source		= interaction.Protagonist.Location;
					
					if (this.CalculateDistance(source, antagonist) <= 2)
					{
	
						if (protagonist.Skills.ContainsKey("Swim"))
						{
							interaction.Log += string.Format("Swim; {0} ", protagonist.Skills["Swim"].Value);
							switch(interaction.ProtagonistCache.SkillCheck("Swim", antagonist.Traits["Movement"].Value, (interaction.Impedance - interaction.Impedance * protagonist.Skills["Swim"].Value)/2, interaction.Impedance/2, EnergyType.Fitness) )
							{
								case SkillResult.PassSufficient: interaction.Log += "You swim through the water. "; this.ApplyInteraction(interaction, protagonist, antagonist); break;
								case SkillResult.PassExhausted: interaction.Failure( "You feel too weak to continue, and are forced to tread water", false); break;
								default: interaction.Log += "You begin to panic as you struggle to keep your head above water "; 
															interaction.Impedance = interaction.Impedance / 2; this.Move(interaction); break;
							}									
						}
						else 
						{
							this.Move(interaction);
							if (interaction.Succeeded)
							{
								using (interaction.Lock(protagonist.Skills)) protagonist.Skills.Add("Swim", new Skill(){ Value = Constants.SkillGrantDefault});	
							}
						}
							                                        
					}
					else interaction.Failure("That is too far away", true);
				}
				else interaction.Failure("Invalid swim", true);
			}
			return interaction;
		}
		
	}
}

