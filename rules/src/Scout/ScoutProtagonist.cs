using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Scout
{
	public class ScoutAntagonist : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//you can only scout a Location
			return (subject is Location);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//Set visibility back to default
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			if (interaction.ProtagonistCache.BurnEnergy(Constants.ScoutCost, true))
			{
				Actor protagonist  = interaction.Protagonist as Actor;
				Location source = protagonist.Location;
				Location target = interaction.Antagonist as Location;
				if (target.Map == source.Map)
				{
					double distance = (target.Coordinates.X - source.Coordinates.X) * (target.Coordinates.X - source.Coordinates.X)
						     + (target.Coordinates.Y - source.Coordinates.Y) * (target.Coordinates.Y - source.Coordinates.Y);
					double detection = protagonist.Skills["Perception"].Value;
					double difficulty = distance * Constants.EdificeScouting;
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
							}		
						}	
					}
				}
		 		else interaction.Failure("You are attempting to scout a location you cannot see", true);
			}
			return interaction;
		}
		#endregion
	}
}