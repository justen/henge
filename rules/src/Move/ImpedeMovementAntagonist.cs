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
			Actor protagonist	= interaction.Protagonist as Actor;
			
			if (antagonist != null && protagonist != null && protagonist.Location.Map == antagonist.Map)
			{
				double impedance = antagonist.Traits.ContainsKey("impedance") ? antagonist.Traits["impedance"].Value : Common.Impedance;
				interaction.Transaction.Add("impedance", impedance);
				interaction.Transaction.Add("aggressorStrength", protagonist.Skills["strength"].Value);
				interaction.Transaction.Add("aggressorEnergy", protagonist.Traits["energy"].Value);

				int dx			= protagonist.Location.Coordinates.X - antagonist.Coordinates.X;
				int dy 			= protagonist.Location.Coordinates.Y - antagonist.Coordinates.Y;
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
				
				//foreach (Edifice structure in antagonist.Structures)
				//{
				//	if ( structure.Traits.ContainsKey("impede") && structure.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(structure);					
				//}
				//
				//foreach (Npc animal in antagonist.Fauna)
				//{
				//	if ( animal.Traits.ContainsKey("impede") && animal.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(animal);	
				//}
				//
				//foreach (Avatar avatar in antagonist.Inhabitants)
				//{
				//	if ( avatar.Traits.ContainsKey("impede") && avatar.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(avatar);	
				//}
				
			}
			else interaction.Failure("Invalid destination", true);
			
			return interaction;
		}
	}
}
