using System;

using Henge.Data.Entities;

namespace Henge.Rules.Interference.Take
{
	public class TakeItemInterference : HengeRule, IInterferer
	{
		public override bool Valid(Component subject)
		{
			//only Actors can interfere
			return subject is Actor;
		}
		
		
		protected override double Visibility(HengeInteraction interaction)
		{
			//Set our visibility back to default * conspicuousness
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			//potentially need to do a skill check here
			//Since this is just a basic "take", let's assume that
			//it's a brute force attempt.
			if (interaction.SubjectCache.SkillCheck("Defend", interaction.ProtagonistCache.Strength - interaction.SubjectCache.Strength))
			{
				double energy = interaction.SubjectCache.Energy;
				
				if (interaction.SubjectCache.UseEnergy(interaction.ProtagonistCache.Energy * interaction.ProtagonistCache.Strength))
				{
					//Managed to prevent the protagonist from taking anything.
					if (interaction.Subject is Npc)
					{
						interaction.Failure(string.Format("{0} prevented you from taking the {1}", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription, interaction.Antagonist.Inspect(interaction.Protagonist).ShortDescription), false);
					}
					else interaction.Failure(string.Format("{0} prevented you from taking the {1}", interaction.Subject.Name, interaction.Antagonist.Inspect(interaction.Protagonist).ShortDescription), false);
				}
				//Whether they overpowered us or not, this is going to cost 'em...
				interaction.ProtagonistCache.UseEnergy(interaction.SubjectCache.Strength * energy);
			}
			
			return interaction;
		}
	}
}

