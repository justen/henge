
using System;
using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Spawn.Character
{


	public class NewAvatarAntagonist : HengeRule, IAntagonist
	{

		public override bool Valid (Component subject)
		{
			//only allow spawning in a Location
			return subject is Location;
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//Don't modify visibility
			return -1;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//nothing to do
			return interaction;
		}
	}
}
