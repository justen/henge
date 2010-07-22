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
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Don't change visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			string impede		= string.Empty;
			Actor subject		= interaction.Protagonist;
			Component target	= interaction.Antagonist;
			Nullable<DateTime> expiry = interaction.Arguments.ContainsKey("expiry")? interaction.Arguments["expiry"]as Nullable<DateTime> : null;
			
			if (interaction.Arguments.ContainsKey("Impede"))
			{
				int dx = interaction.Arguments.ContainsKey("dx") ? (int)interaction.Arguments["dx"] : 0; 
				int dy = interaction.Arguments.ContainsKey("dy") ? (int)interaction.Arguments["dy"] : 0;
				impede = string.Format("{1}{0}", (dx > 0) ? 'e' : (dx < 0) ? 'w' : '-', (dy > 0) ? 's' : (dy < 0) ? 'n' : '-' );
				
				interaction.Deltas.Add((success) => {
					if (!subject.Traits.ContainsKey("Impede")) subject.Traits.Add("Impede", new Trait(double.MaxValue, 0, 0));
					Trait trait		= subject.Traits["Impede"];
					trait.Expiry 	= expiry;
					trait.Flavour	= impede;
					trait.Subject	= target;
					return true;
				});
			}
			
			if (interaction.Arguments.ContainsKey("Guard"))
			{
				interaction.Deltas.Add((success) => {
					if (!subject.Traits.ContainsKey("Guard")) subject.Traits.Add("Guard", new Trait(double.MaxValue, 0, 0));
					Trait trait		= subject.Traits["Guard"];
					trait.Expiry	= expiry;
					trait.Flavour	= String.Empty;
					trait.Subject	= target;
					return true;
				});	
			}
			
			return impede.Length > 0 ?
				interaction.Success(string.Format("Defending {0} from the {1}", target.Inspect(subject).ShortDescription, impede)) :
				interaction.Success(string.Format("Defending {0}", target.Inspect(subject).ShortDescription));
		}
	}
}