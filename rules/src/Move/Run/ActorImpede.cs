using System;
using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move.Run
{
	public class ActorImpede : InterferenceRule
	{
		public ActorImpede()
		{
		}
		
		protected Actor subject;
		
		public override double Priority (Henge.Data.Entities.Component subject)
		{
			double result = -1;
			if (subject is Actor)
			{
				this.subject = (Actor)subject;
				result = 1;
			}
			return result;
		}

		
		public override Interaction Apply(Interaction interaction)
		{
			
			//only need to do this skill check if the protagonist hasn't already been stopped
			if ((double)interaction.Transaction["impedance"] < (double)interaction.Transaction["aggressorEnergy"]) 
			{
				double strength = this.subject.Skills.ContainsKey("strength") ? this.subject.Skills["strength"].Value : Common.DefaultSkill;
				//can only intervene if not exhausted
				if (Common.GetEnergy(this.subject).Value > 0)
				{
					if (Common.SkillCheck(this.subject, "defend", 2.0 * (double)interaction.Transaction["aggressorStrength"] - strength))
					{
						if ( Common.UseEnergy(this.subject, (double)interaction.Transaction["aggressorStrength"] *  (double)interaction.Transaction["aggressorEnergy"]) )
						{
							double impedance = this.subject.Traits.ContainsKey("weight") ? this.subject.Traits["weight"].Value : Common.ActorBaseWeight;
							interaction.Transaction["impedance"] = (double)interaction.Transaction["impedance"] + impedance * Common.WeightToImpedance;	
						}
					}
					else
					{
						//brute force & ignorance time...
						if ( Common.UseEnergy(this.subject, 2.0 * (double)interaction.Transaction["aggressorStrength"] *  (double)interaction.Transaction["aggressorEnergy"]) )
						{
							double impedance = this.subject.Traits.ContainsKey("weight") ? this.subject.Traits["weight"].Value : Common.ActorBaseWeight;
							interaction.Transaction["impedance"] = (double)interaction.Transaction["impedance"] + impedance * Common.WeightToImpedance * 0.5;	
						}
					}
				}
			}
			return interaction;
			
		}
	}
}
