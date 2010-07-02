using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Search
{
	public class SearchProtagonist : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can search
			return (subject is Actor);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override HengeInteraction Apply (HengeInteraction interaction)
		{
			if (!interaction.Finished)
			{
				Actor protagonist = interaction.Protagonist as Actor;
				List<Component> items = interaction.Arguments["Items"] as List<Component>;
				int i = 0;
				while (interaction.ProtagonistCache.BurnEnergy(Constants.SearchCost, false))
				{
					if (i<items.Count)
					{
						//need to do a search-based skill check for each item
						Item item = items[i] as Item;
						if (interaction.ProtagonistCache.SkillCheck("Search", this.CalculateDifficulty(item)))
						{
							//found something, set its visibility
							interaction.Deltas.Add((success) => {
								item.Traits["Visibility"].Value = Constants.HighVisibility;
								return true;
							});
							interaction.Success(string.Format("You found something! You put the {0} in plain view", item.Inspect(protagonist).ShortDescription));
							break;
						}
					}
					i++;
				}
				if (!interaction.Finished) interaction.Failure("Exhausted, you give up the search", false);
			}
			return interaction;
		}
		#endregion
		
		protected virtual double CalculateDifficulty(Item item)
		{
			return (1 - item.Traits["Visibility"].Value / Constants.SearchDifficulty);
		}
	}
}

