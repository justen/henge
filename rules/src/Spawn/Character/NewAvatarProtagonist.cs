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
			
			using (interaction.Lock(location.Inhabitants, avatar.Skills))
			{
				location.Inhabitants.Add(avatar);
				avatar.Skills.Add("Strength",	new Skill() { Value = Constants.StartingSkill });
				avatar.Skills.Add("Climb",		new Skill() { Value = Constants.StartingSkill });
				avatar.Skills.Add("Fitness",	new Skill() { Value = Constants.StartingSkill });
			}
			
			return interaction.Success("Spawned");
		}
	}
}
