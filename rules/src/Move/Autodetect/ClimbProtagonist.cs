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
			if(! interaction.Finished)
			{
				Location antagonist	= interaction.Antagonist as Location;
				Location source		= interaction.Protagonist.Location;
				if (interaction.Protagonist != null && antagonist != null)
				{
					if (this.CalculateDistance(source, antagonist) <= 2)
					{

						double difficulty = interaction.Antagonist.Traits["Movement"].Value * (double)(antagonist.Z - source.Z)/255;
						if (difficulty<0) difficulty = -difficulty;
			
						if (interaction.Arguments.ContainsKey("Climb")) difficulty *= (double)interaction.Arguments["Climb"];
						interaction.Log+=string.Format("Climb  {0}. " , interaction.Protagonist.Skills["Climb"].Value);
			
						double cost = interaction.Impedance * 2 - interaction.Antagonist.Traits["Impede"].Value * 1;
						switch(interaction.ProtagonistCache.SkillCheck("Climb", difficulty, cost, cost, EnergyType.Strength))
						{
						case SkillResult.PassSufficient: 
								interaction.Log+="You climb confidently. ";
								this.ApplyInteraction(interaction, interaction.Protagonist, antagonist);
								break;
						case SkillResult.PassExhausted:
								interaction.Failure("You start climbing up a likely route but are not strong enough to follow it", false);
								break;
						case SkillResult.FailSufficient:
								interaction.Failure("You are outmatched by the climb", false);
								break;
						case SkillResult.FailExhausted: 
								interaction.Failure("The climb is totally beyond you", false);
								break;
						}
					}
					else interaction.Failure("That is too far away", true);
				}
				else interaction.Failure("Invalid climb", true);
			}
			return interaction;
		}
	}
}

