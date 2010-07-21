using System;
using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Spawn.Character
{
	public class NewAvatar : HengeRule, IProtagonist
	{

		public override bool Valid (Component subject)
		{
			
			return subject is Henge.Data.Entities.Avatar;
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Set our visibility back to default * conspicuousness
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * Constants.BaseConspicuousness);
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Avatar avatar		= interaction.Protagonist as Avatar;
			Location location	= interaction.Antagonist as Location;
			
			interaction.Deltas.Add((success) => {
				location.Inhabitants.Add(avatar);
				avatar.Traits.Add("Energy", new Trait { Value = 10, Minimum = -10, Maximum = 10 });
				avatar.Skills.Add("Strength", new Skill {Value = 0.5});
				avatar.Traits.Add("Weight", new Trait { Value = Constants.ActorBaseWeight, Minimum = 0 });
				return true;
			});
			
			return interaction.Success("Spawned");
		}
	}
}
