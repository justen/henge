using System;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Eat
{
	public class EatProtagonist : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can eat stuff
			return (subject is Actor);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Eating doesn't affect your visibility
			subject = null;
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			if (!interaction.Finished && this.Validate(interaction))
			{
				Item food			= interaction.Antagonist as Item;
				Actor actor 		= interaction.Protagonist as Actor;
				double nutrition	= food.Traits["Nutrition"].Value;
				
				if (nutrition > 0) 
				{
					interaction.Success(string.Format("You eat the {0} with relish", food.Inspect(actor)));
				}
				else 
				{
					if (nutrition == 0) interaction.Success(string.Format("You eat the {0}", food.Inspect(actor)));
					else interaction.Success(string.Format("You eat the {0}. You feel queasy.", food.Inspect(actor)));
				}
				
				if (food.Owner == actor)
				{
					Trait weight 		= actor.Traits["Weight"];
					Trait constitution	= actor.Traits["Constitution"];
					
					using (interaction.Lock(actor.Inventory, weight, constitution))
					{
						weight.SetValue(weight.Value - food.Traits["Weight"].Value);
						constitution.SetValue(constitution.Value + nutrition);
						actor.Inventory.Remove(food);
					}
					interaction.Delete(food);	
				}
				else interaction.Failure("You no longer have that", true);
				
			}
			return interaction;
		}
		#endregion
	}
}

