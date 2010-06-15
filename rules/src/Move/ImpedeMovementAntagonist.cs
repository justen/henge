
using System;

using System.Collections.Generic;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Move.Run
{


	public class BasicRunAntagonist : AntagonistRule
	{

		public BasicRunAntagonist ()
		{
		}
		
		public override double Priority(Component subject)
		{
			return (subject is Location) ? 1 : -1;
		}
		
		
		public override Interaction Apply(Interaction interaction)
		{	
			if ((interaction.Antagonist is Location)&& (interaction.Protagonist.Location.Map == ((Location)interaction.Antagonist).Map))
			{
	
				double impedance = interaction.Antagonist.Traits.ContainsKey("impedance") ? interaction.Antagonist.Traits["impedance"].Value : Common.Impedance;
				interaction.Transaction.Add("impedance", impedance);
				interaction.Transaction.Add("aggressorStrength", interaction.Protagonist.Skills["strength"]);
				interaction.Transaction.Add("aggressorEnergy", interaction.Protagonist.Traits["energy"]);
				
				int dX = interaction.Protagonist.Location.Coordinates.X - ((Location)interaction.Antagonist).Coordinates.X;
				int dY = interaction.Protagonist.Location.Coordinates.Y - ((Location)interaction.Antagonist).Coordinates.Y;
				char[] impede = new char[2];
				int i = 0;
				if (dY > 0) impede[i++] = 's';
				if (dY < 0) impede[i++] = 'n';
				if (dX > 0) impede[i++] = 'e';
				if (dX < 0) impede[i++] = 'w';
				
				foreach (Edifice structure in ((Location)interaction.Antagonist).Structures)
				{
					if ( structure.Traits.ContainsKey("impede") && structure.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(structure);					
				}
				
				foreach (Npc animal in ((Location)interaction.Antagonist).Fauna)
				{
					if ( animal.Traits.ContainsKey("impede") && animal.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(animal);	
				}
				
				foreach (Avatar avatar in ((Location)interaction.Antagonist).Inhabitants)
				{
					if ( avatar.Traits.ContainsKey("impede") && avatar.Traits["impede"].Flavour.IndexOfAny(impede) > -1 ) interaction.Interferers.Add(avatar);	
				}
				
			}
			else interaction.Failure("Invalid destination", true);
			return interaction;
		}
	}
}
