using System;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Eat
{
	public class EatAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//you can only eat Items
			return (subject is Item);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//You're not going to eat the subject, so leave its visibility alone
			subject = null;
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			if (!interaction.Antagonist.Traits.ContainsKey("Nutrition"))
				interaction.Failure("After a mighty struggle with yourself, you are forced to admit that you cannot bring yourself to try to eat that", false);
			return interaction;
		}
		#endregion
	}
}

