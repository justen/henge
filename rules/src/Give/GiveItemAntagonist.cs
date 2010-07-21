using System;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Give
{
	public class GiveItemAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid(Component subject)
		{
			//you can give an item to anything, so...
			return true;
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Don't change visibility
			subject = null;
			return -1;
		}
		

		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//nothing to do here
			return interaction;
		}
	}
}

