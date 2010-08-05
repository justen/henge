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

		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Set our visibility back to default * conspicuousness
			subject = interaction.Subject;
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//check to see if the impede trait has expired on this subject:
			if (this.Validate(interaction))
			{
				if (interaction.TraitCheck(interaction.Subject, "impede"))
				{
					//Check the actor is actually trying to defend whatever we're interested in
					if (interaction.Subject.Traits["Impede"].Subject == interaction.Antagonist )
					{
						// Only need to do this skill check if the protagonist hasn't already been stopped
						if (interaction.Impedance < interaction.ProtagonistCache.Energy) 
						{
							double strength = interaction.SubjectCache.Strength;
							
							// Can only intervene if not exhausted
							if (interaction.SubjectCache.Energy > 0)
							{
								switch( interaction.SubjectCache.SkillCheck("Defend", 2 * interaction.ProtagonistCache.Strength - strength, interaction.ProtagonistCache.Strength * interaction.ProtagonistCache.Energy, 2 * interaction.ProtagonistCache.Strength * interaction.ProtagonistCache.Energy, EnergyType.Strength))
								{
									case SkillResult.PassSufficient:
										if (interaction.Subject is Avatar) interaction.Log+=string.Format("{0} grapples with you, hindering your progress. ", interaction.Subject.Name);
										else interaction.Log+=string.Format("A {0} hinders your progress", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription);
										interaction.Impedance += interaction.SubjectCache.Weight * Constants.WeightToImpedance;
										break;
									case SkillResult.FailSufficient:
										if (interaction.Subject is Avatar) interaction.Log+=string.Format("{0} grapples with you, hindering your progress. ", interaction.Subject.Name);
										else interaction.Log+=string.Format("A {0} hinders your progress", interaction.Subject.Inspect(interaction.Protagonist).ShortDescription);
										interaction.Impedance += interaction.SubjectCache.Weight * Constants.WeightToImpedance;
										break;		
								}
							}
						}
					}
				}
			}
	
			return interaction;
			
		}
	}
}
