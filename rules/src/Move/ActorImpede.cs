using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class ActorImpede : HengeRule, IInterferer
	{
		public override bool Valid(Component subject)
		{
			return subject is Actor;
		}

		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			//check to see if the impede trait has expired on this subject:
			if (interaction.TraitCheck(interaction.Subject, "impede"))
			{
				// Only need to do this skill check if the protagonist hasn't already been stopped
				if (interaction.Impedance < interaction.ProtagonistCache.Energy) 
				{
					double strength = interaction.SubjectCache.Strength;
					
					// Can only intervene if not exhausted
					if (interaction.SubjectCache.Energy > 0)
					{
						if (interaction.SubjectCache.SkillCheck("Defend", 2 * interaction.ProtagonistCache.Strength - strength))
						{
							if (interaction.SubjectCache.UseEnergy(interaction.ProtagonistCache.Strength * interaction.ProtagonistCache.Energy))
							{
								interaction.Impedance += interaction.SubjectCache.Weight * Constants.WeightToImpedance;
							}
						}
						else
						{
							if (interaction.SubjectCache.UseEnergy(2 * interaction.ProtagonistCache.Strength * interaction.ProtagonistCache.Energy))
							{
								interaction.Impedance += interaction.SubjectCache.Weight * Constants.WeightToImpedance;
							}
						}
					}
				}
			}
	
			return interaction;
			
		}
	}
}
