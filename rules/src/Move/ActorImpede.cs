using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	/*public class ActorImpede : InterferenceRule
	{
		public override double Priority (Component subject)
		{
			return (subject is Actor) ? 1 : -1;
		}

		
		public override Interaction Apply(Interaction interaction)
		{
			Actor subject = interaction.Subject as Actor;
			
			if (subject != null)
			{
				double impedance = (double)interaction.Transaction["impedance"];
				
				// Only need to do this skill check if the protagonist hasn't already been stopped
				if (impedance < (double)interaction.Transaction["aggressorEnergy"]) 
				{
					double strength = subject.Skills.ContainsKey("strength") ? subject.Skills["strength"].Value : Common.DefaultSkill;
					
					// Can only intervene if not exhausted
					if (Common.GetEnergy(subject).Value > 0)
					{
						if (Common.SkillCheck(subject, "defend", 2.0 * (double)interaction.Transaction["aggressorStrength"] - strength))
						{
							if ( Common.UseEnergy(subject, (double)interaction.Transaction["aggressorStrength"] *  (double)interaction.Transaction["aggressorEnergy"]) )
							{
								double weight = subject.Traits.ContainsKey("weight") ? subject.Traits["weight"].Value : Common.ActorBaseWeight;
								interaction.Transaction["impedance"] = impedance + weight * Common.WeightToImpedance;	
							}
						}
						else
						{
							//brute force & ignorance time...
							if ( Common.UseEnergy(subject, 2.0 * (double)interaction.Transaction["aggressorStrength"] *  (double)interaction.Transaction["aggressorEnergy"]) )
							{
								double weight = subject.Traits.ContainsKey("weight") ? subject.Traits["weight"].Value : Common.ActorBaseWeight;
								interaction.Transaction["impedance"] = impedance + weight * Common.WeightToImpedance * 0.5;	
							}
						}
					}
				}
			}
			return interaction;
			
		}
	}*/
}
