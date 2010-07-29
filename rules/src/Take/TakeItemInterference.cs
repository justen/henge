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
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Set our visibility back to default * conspicuousness
			subject = interaction.Subject;
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			//potentially need to do a skill check here
			//Since this is just a basic "take", let's assume that
			//it's a brute force attempt.
			double energy = interaction.SubjectCache.Energy;
			if (interaction.Subject is Npc)
			{
				interaction.Log += string.Format("A {0} attempted to prevent you from taking the {1} ", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription, interaction.Antagonist.Inspect(interaction.Protagonist).ShortDescription);
			}
			else interaction.Failure(string.Format("{0} attempted to prevent you from taking the {1} ", interaction.Subject.Name, interaction.Antagonist.Inspect(interaction.Protagonist).ShortDescription), false);
			bool charge = false;
			switch (interaction.SubjectCache.SkillCheck("Defend", interaction.ProtagonistCache.Strength - interaction.SubjectCache.Strength, interaction.ProtagonistCache.Energy * interaction.ProtagonistCache.Strength, 0, EnergyType.Strength))
			{
				
			
			case SkillResult.PassSufficient: 
					interaction.Failure("and succeeded.", false);
					charge = true;
					break;
			case SkillResult.PassExhausted:
					//Whether they overpowered us or not, this is going to cost 'em...
					interaction.Log+=("but you overpowered them. ");
					charge = true;
					break;
			default: interaction.Log+=("but failed. ");
					break;

			}
			if (charge)	interaction.ProtagonistCache.UseEnergy(interaction.SubjectCache.Strength * energy, EnergyType.Strength);
			return interaction;
		}
	}
}

