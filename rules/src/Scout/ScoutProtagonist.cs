using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Scout
{
	public class ScoutAntagonist : HengeRule, IProtagonist
	{
		public override bool Valid(Component subject)
		{
			//you can only scout a Location
			return (subject is Location);
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Set visibility back to default
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction) )
			{
				if ( interaction.ProtagonistCache.Energy > 0)
				{
					Actor protagonist	= interaction.Protagonist as Actor;
					Location source		= protagonist.Location;
					Location target		= interaction.Antagonist as Location;
					
					if (target.Map == source.Map)
					{
						if (source.CanSee(target))
						{
							double dx			= target.X - source.X;
							double dy			= target.Y - source.Y;
							double distance		= dx * dx + dy * dy;
							double sourceCover	= source.Traits.ContainsKey("Cover") ? source.Traits["Cover"].Value : Constants.DefaultCover;
							double targetCover	= target.Traits.ContainsKey("Cover") ? target.Traits["Cover"].Value : Constants.DefaultCover;
							distance			= distance * Math.Max(sourceCover, targetCover);
							double detection	= protagonist.Skills["Perception"].Value; 
							double difficulty	= distance * Constants.EdificeScouting;
							int success = 0;
							bool weary = false;
							
							switch (interaction.ProtagonistCache.SkillCheck("Perception", distance * Constants.LocationScouting, Constants.ScoutCost, Constants.ScoutCost, EnergyType.Concentration))
							{
							case SkillResult.PassSufficient:
									interaction.Results.Add("Location", target);
									success=1;
									break;
							case SkillResult.FailSufficient: break;
							default: weary = true; break;
							}
							switch (interaction.ProtagonistCache.SkillCheck("Perception", difficulty, Constants.ScoutCost, Constants.ScoutCost, EnergyType.Concentration))
							{
							case SkillResult.PassSufficient:
									success++;
									interaction.Results.Add("Structures", target.Structures.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= detection - difficulty).ToList());
									difficulty = distance * Constants.ActorScouting;
									break;
							case SkillResult.FailSufficient: break;
							default: weary = true; break;
							}
							switch (interaction.ProtagonistCache.SkillCheck("Perception", difficulty, Constants.ScoutCost, Constants.ScoutCost, EnergyType.Concentration))
							{
							case SkillResult.PassSufficient:
										difficulty = detection - difficulty;
										interaction.Results.Add("NPCs", target.Fauna.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= difficulty).ToList());
										interaction.Results.Add("Avatars", target.Inhabitants.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= difficulty).ToList());
										success ++;
										break;
							case SkillResult.FailSufficient: break;
							default: weary = true; break;
							}
							if (weary) interaction.Log+="Your tired eyes may not be trustworthy, but you";
							else interaction.Log+= "You ";
							switch (success)
							{
							case 3: interaction.Success("feel that you have a good view of the area");
									break;
							case 2: interaction.Success("can see the area reasonably well");
									break;
							case 1: interaction.Success("can just about make out the terrain");
									break;
							default: interaction.Failure("can't make anything out", false); break;
							}
						}
						else interaction.Failure("You can't see from here", false);
					}
			 		else interaction.Failure("You are attempting to scout a location you cannot see", true);
				} else interaction.Failure("You are too tired to focus on scouting", false);
			}
			return interaction;
		}
	}
}