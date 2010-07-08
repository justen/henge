using System;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Metabolise
{
	public class MetaboliseAntagonist : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can metabolise
			return (subject is Actor);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//This does not affect visibility
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			//TODO: Need to check if the Actor is a logged-out Avatar and give them bonuses if so.
			if (!interaction.Finished)
			{
				Actor actor = interaction.Protagonist as Actor;
				Trait health = actor.Traits["Health"];
				Trait energy = actor.Traits["Energy"];
				Trait constitution = actor.Traits["Constitution"];
				string message = "You rest and recuperate";
				if (health.Flavour!="Dead")
				{
					double constDelta = Constants.Tick.MetabolicRate;
					double healthDelta = Constants.Tick.Healthy.Heal;
					double energyDelta = Constants.Tick.Healthy.Revitalise;
					
					if (constitution.Value <= 0)
					{
						//decrease Health, increase Energy by less
						healthDelta = Constants.Tick.Ill.Heal;
						energyDelta = Constants.Tick.Ill.Revitalise;
						constDelta = -constDelta;
						if (constitution.Value == 0)
						{// increase Constitution
							constDelta = 0;
						}
						message = "You feel weaker";
					}
					if (health.Value+healthDelta<0)
					{
						interaction.Deltas.Add((success) => {
							health.Value = 0;
							energy.Value = 0;
							constitution.Value = 0;
							health.Flavour = "Dead";
							return true;
						});
						interaction.Failure("You succumb to your ailments. Your story is over... will your line continue?", false);
					}
					else
					{
						interaction.Deltas.Add((success) => {
							health.Value += healthDelta;
							energy.Value += energyDelta;
							constitution.Value += constDelta;
							return true;
						});
						interaction.Success(message);
					}
					
				}
				else interaction.Failure("Your family await a new leader to come of age", false);
				
			}
			return interaction;
		}
		#endregion
	}
}