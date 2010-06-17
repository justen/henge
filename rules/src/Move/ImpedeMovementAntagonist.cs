using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Move
{
	public class ImpedeMovement : AntagonistRule
	{
		public override double Priority(Component subject)
		{
			return (subject is Location) ? 1 : -1;
		}
		
		
		public override Interaction Apply(Interaction interaction)
		{	
			Location antagonist = interaction.Antagonist as Location;
			
			if (antagonist != null && interaction.Protagonist.Location.Map == antagonist.Map)
			{
				double impedance = interaction.Antagonist.Traits.ContainsKey("impedance") ? interaction.Antagonist.Traits["impedance"].Value : Common.Impedance;
				interaction.Transaction.Add("impedance", impedance);
				interaction.Transaction.Add("aggressorStrength", interaction.Protagonist.Skills["strength"].Value);
				interaction.Transaction.Add("aggressorEnergy", interaction.Protagonist.Traits["energy"].Value);

				int dx			= interaction.Protagonist.Location.Coordinates.X - antagonist.Coordinates.X;
				int dy 			= interaction.Protagonist.Location.Coordinates.Y - antagonist.Coordinates.Y;
				char [] impede 	= new char [] {
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
				
				/*foreach (Edifice structure in antagonist.Structures)
				{
					if ( structure.Traits.ContainsKey("impede") && structure.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(structure);					
				}
				
				foreach (Npc animal in antagonist.Fauna)
				{
					if ( animal.Traits.ContainsKey("impede") && animal.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(animal);	
				}
				
				foreach (Avatar avatar in antagonist.Inhabitants)
				{
					if ( avatar.Traits.ContainsKey("impede") && avatar.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(avatar);	
				}*/
				
			}
			else interaction.Failure("Invalid destination", true);
			
			return interaction;
		}
	}
}
