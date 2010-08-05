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
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This does not affect visibility
			subject = null;
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			//TODO: Need to check if the Actor is a logged-out Avatar and give them bonuses if so.
			if (this.Validate(interaction) && !interaction.Finished)
			{
				Actor actor			= interaction.Protagonist as Actor;
				Trait health		= actor.Traits["Health"];
				Trait energy		= actor.Traits["Energy"];
				Trait constitution	= actor.Traits["Constitution"];
				string message		= "You rest and recuperate";

				double constDelta	= Constants.Tick.MetabolicRate;
				double healthDelta	= Constants.Tick.Healthy.Heal;
				double energyDelta	= Constants.Tick.Healthy.Revitalise;
				
				if (constitution.Value <= 0)
				{
					// Decrease Health, increase Energy by less
					healthDelta	= Constants.Tick.Ill.Heal;
					energyDelta	= Constants.Tick.Ill.Revitalise;
					constDelta	= -constDelta;
					message		= "You feel weaker";
					
					// Increase Constitution
					if (constitution.Value == 0) constDelta = 0;		
				}
				
				if (health.Value + healthDelta < 0)
				{
					using (interaction.Lock(health, energy, constitution))
					{
						health.SetValue(0);
						energy.SetValue(0);
						constitution.SetValue(0);
						health.Flavour = "Dead";
					}
					interaction.Failure("You succumb to your ailments. Your story is over... will your line continue?", false);
				}
				else
				{
					using (interaction.Lock(health, energy, constitution))
					{
						health.SetValue(health.Value + healthDelta);
						energy.SetValue(energy.Value + energyDelta);
						constitution.SetValue(constitution.Value + constDelta);
					}
					interaction.Success(message);
				}
			}
			return interaction;
		}
		#endregion
	}
}