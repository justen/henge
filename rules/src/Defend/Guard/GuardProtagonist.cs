using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Defend.Guard
{
	public class Guard : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//This is the rule for any old Actor
			return subject is Actor;
		}
		
		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			Actor subject = interaction.Protagonist;
			Component target = interaction.Antagonist;
			Nullable<DateTime> expiry = interaction.Arguments.ContainsKey("expiry")? interaction.Arguments["expiry"]as Nullable<DateTime> : null;
			int dx = interaction.Arguments.ContainsKey("dx")? (int) interaction.Arguments["dx"] : 0; 
			int dy = interaction.Arguments.ContainsKey("dy")? (int) interaction.Arguments["dy"] : 0;
			string impede = string.Format("{0}{1}",	(dx > 0) ? 'e' : (dx < 0) ? 'w' : '-',
													(dy > 0) ? 's' : (dy < 0) ? 'n' : '-' );
			interaction.Deltas.Add((success) => {
				if (!subject.Traits.ContainsKey("impede")) subject.Traits.Add("impede", new Trait());
				Trait trait = subject.Traits["impede"];
				trait.Expiry = expiry;
				trait.Flavour = impede;
				trait.Subject = target;
				return true;
			});
			interaction.Success(string.Format("Defending {0}", impede));
			return interaction;
		}
		
	}
}