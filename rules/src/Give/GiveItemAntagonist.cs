using System;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Give
{
	public class GiveItemAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//you can give an item to anything, so...
			return true;
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//Don't change visibility
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override HengeInteraction Apply (HengeInteraction interaction)
		{
			//nothing to do here
			return interaction;
		}
		
		#endregion
	}
}
