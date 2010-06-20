using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class ActorImpede : HengeRule, IInterferer
	{
		public override double Priority (Component subject)
		{
			return (subject is Actor) ? 1 : -1;
		}

		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			Actor subject = interaction.Subject as Actor;
			
			if (subject != null)
			{
				// Only need to do this skill check if the protagonist hasn't already been stopped
				if (interaction.Impedance < interaction.Energy) 
				{
					double strength = subject.Skills.ContainsKey("Strength") ? subject.Skills["Strength"].Value : Constants.DefaultSkill;
					
					// Can only intervene if not exhausted
					if (subject.Traits["Energy"].Value > 0)
					{
						if (interaction.SkillCheck(subject, "Defend", 2 * interaction.Strength - strength))
						{
							if (interaction.UseEnergy(subject, interaction.Strength * interaction.Energy))
							{
								double weight = subject.Traits.ContainsKey("Weight") ? subject.Traits["Weight"].Value : Constants.ActorBaseWeight;
								interaction.Impedance += weight * Constants.WeightToImpedance;
							}
						}
						else
						{
							if (interaction.UseEnergy(subject, 2 * interaction.Strength * interaction.Energy))
							{
								double weight = subject.Traits.ContainsKey("Weight") ? subject.Traits["Weight"].Value : Constants.ActorBaseWeight;
								interaction.Impedance += weight * Constants.WeightToImpedance;
							}
						}
					}
				}
			}
			return interaction;
			
		}
	}
}
