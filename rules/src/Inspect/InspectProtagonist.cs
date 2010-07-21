using System;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Inspect
{
	public class InspectProtagonist : HengeRule, IProtagonist
	{
		public override bool Valid(Component subject)
		{
			//Only Actors can do inspecting
			return subject is Actor;
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//This shouldn't affect your visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (!interaction.Finished)
			{
				if (interaction.AntagonistCache.BurnEnergy(Constants.InspectionCharge, false))
				{
					interaction.Success(string.Format("You examine it closely and see: {0}", interaction.Antagonist.Inspect(interaction.Protagonist).Description));					
				} 
				else 
				{
					interaction.Failure("You're too exhausted to concentrate", false);
				}
			}
			return interaction;
		}
	}
}

