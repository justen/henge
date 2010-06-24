using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Move
{
	public class ImpedeMovement : HengeRule, IAntagonist
	{
		public override bool Valid(Component subject)
		{
			return subject is Location;
		}
		
		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{	
			Location antagonist = interaction.Antagonist as Location;
			Actor protagonist	= interaction.Protagonist;
			
			if (antagonist != null && protagonist != null && protagonist.Location.Map == antagonist.Map)
			{
				interaction.Impedance	= antagonist.Traits.ContainsKey("impede") ? antagonist.Traits["impede"].Value : Constants.Impedance;
				int dx					= protagonist.Location.Coordinates.X - antagonist.Coordinates.X;
				int dy 					= protagonist.Location.Coordinates.Y - antagonist.Coordinates.Y;
				char [] impede 			= new char [] {
					(dx > 0) ? 'e' : (dx < 0) ? 'w' : '-',
					(dy > 0) ? 's' : (dy < 0) ? 'n' : '-'
				};
				
				antagonist.Structures
					.Where(c => c.Traits.ContainsKey("impede") && c.Traits["impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => interaction.Interferers.Add(c));
				
				antagonist.Fauna
					.Where(c => c.Traits.ContainsKey("impede") && c.Traits["impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => interaction.Interferers.Add(c));
				
				antagonist.Inhabitants
					.Where(c => c.Traits.ContainsKey("impede") && c.Traits["impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => interaction.Interferers.Add(c));
			}
			else interaction.Failure("Invalid destination", true);
			
			return interaction;
		}
	}
}
