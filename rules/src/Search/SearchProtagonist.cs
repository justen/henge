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
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
		}
		

		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (!interaction.Finished && this.Validate(interaction))
			{
				Actor protagonist		= interaction.Protagonist as Actor;
				List<Component> items	= interaction.Arguments["Items"] as List<Component>;
				
				int i = 0;
				bool stillLooking = true;
				do
				{
					if (i < items.Count)
					{
						Item item = items[i] as Item;
						switch (interaction.ProtagonistCache.SkillCheck("Search", this.CalculateDifficulty(item), Constants.SearchCost, Constants.SearchCost, EnergyType.Concentration))
						{
							case SkillResult.PassSufficient:
								using (interaction.Lock(item.Traits["Visibility"])) item.Traits["Visibility"].SetValue(Constants.HighVisibility);
								interaction.Success(string.Format("You found something! You put the {0} in plain view", item.Inspect(protagonist).ShortDescription));
								stillLooking = false;
								break;
							case SkillResult.FailSufficient:
								i++;
								break;
							default: 
								interaction.Failure("Exhausted, you give up the search.", false);
								stillLooking = false;
								break;
						}
					}
					else
					{
						if(!interaction.ProtagonistCache.UseEnergy(Constants.SearchCost, EnergyType.Concentration))
						{
							interaction.Failure("Exhausted, you give up the search.", false);
							stillLooking = false;
						}
					}
				} while (stillLooking);
			}
			return interaction;
		}

		
		protected virtual double CalculateDifficulty(Item item)
		{
			return (1 - item.Traits["Visibility"].Value / Constants.SearchDifficulty);
		}
	}
}

