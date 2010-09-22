using System;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Metabolise
{
	public class MetaboliseAntagonist : TickRule, IAntagonist
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
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//TODO: Need to check if the Actor is a logged-out Avatar and give them bonuses if so.
			if (this.Validate(interaction) && !interaction.Finished)
			{
				Actor actor			= interaction.Antagonist as Actor;
				Trait health		= actor.Traits["Health"];
				Trait energy		= actor.Traits["Energy"];
				Trait reserve		= actor.Traits["Reserve"];
				Trait constitution	= actor.Traits["Constitution"];
				string message		= "you rested and recuperated a little";
				double constDelta	= Constants.Tick.MetabolicRate;
				double healthDelta	= (health.Value < health.Maximum) ? Constants.Tick.Healthy.Heal : 0;
				double energyDelta	= (reserve.Value < reserve.Maximum) ? Constants.Tick.Healthy.Revitalise : 0;
				constDelta+= (healthDelta + energyDelta);
				
				if (constitution.Value <= 0)
				{
					// Decrease Health, increase Energy by less
					healthDelta	= Constants.Tick.Ill.Heal;
					energyDelta	= Constants.Tick.Ill.Revitalise;
					constDelta	= -constDelta;
					message		= "you felt weaker";
					
					// Increase Constitution
					if (constitution.Value == 0) constDelta = 0;		
				}
				
				if (health.Value + healthDelta < 0)
				{
					using (interaction.Lock(health, reserve, energy, constitution))
					{
						health.SetValue(0);
						energy.SetValue(0);
						reserve.SetValue(0);
						constitution.SetValue(constitution.Minimum);
						health.Flavour = "Dead";
					}
					interaction.Success("you succumbed to your ailments. Your story is over... will your line continue?");
				}
				else
				{
					using (interaction.Lock(health, reserve, constitution))
					{
						health.SetValue(health.Value + healthDelta);
						reserve.SetValue(reserve.Value + energyDelta);
						constitution.SetValue(constitution.Value - constDelta);
					}
					interaction.Success(message);
				}	
			}
			return interaction;
		}
		#endregion
	}
}