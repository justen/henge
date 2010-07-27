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
			if (interaction.ProtagonistCache.BurnEnergy(Constants.ScoutCost, true))
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
						
						if (interaction.ProtagonistCache.SkillCheck("Perception", distance * Constants.LocationScouting))
						{
							interaction.Results.Add("Location", target);
							
							if (interaction.ProtagonistCache.SkillCheck("Perception", difficulty))
							{
								interaction.Results.Add("Structures", target.Structures.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= detection - difficulty).ToList());
								difficulty = distance * Constants.ActorScouting;
								
								if (interaction.ProtagonistCache.SkillCheck("Perception", difficulty))
								{
									difficulty = detection - difficulty;
									interaction.Results.Add("NPCs", target.Fauna.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= difficulty).ToList());
									interaction.Results.Add("Avatars", target.Inhabitants.Where(c => c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value >= difficulty).ToList());
									interaction.Success("You feel that you have a good view of the area");
								}
								else interaction.Success("You can see the area reasonably well");
							}
							else interaction.Success("You can just about make out the terrain");
						}
						else interaction.Failure("You can't make anything out", false);
					}
					else interaction.Failure("You can't see from here", false);
				}
		 		else interaction.Failure("You are attempting to scout a location you cannot see", true);
			}
			return interaction;
		}
	}
}