using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Communicate.Talk
{
	public class ToIPerson : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Talking to a character
			return (subject is Avatar);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//visibility doesn't change
			subject = null;
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				List<Avatar> target = new List<Avatar>();
				target.Add(interaction.Antagonist as Avatar);
				interaction.Arguments.Add("Recipients", target);
			}
			return interaction;
		}
		#endregion
	}
}