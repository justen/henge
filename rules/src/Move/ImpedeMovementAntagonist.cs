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
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Do not change visibility
			subject = null;
			return -1;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{	
			Location antagonist = interaction.Antagonist as Location;
			Actor protagonist	= interaction.Protagonist;
			
			if (antagonist != null && protagonist != null && protagonist.Location.Map == antagonist.Map)
			{
				interaction.Impedance	= antagonist.Traits.ContainsKey("Impede") ? antagonist.Traits["Impede"].Value : Constants.Impedance;
				int dx					= protagonist.Location.X - antagonist.X;
				int dy 					= protagonist.Location.Y - antagonist.Y;
				char [] impede 			= new char [] {
					(dx > 0) ? 'e' : (dx < 0) ? 'w' : '-',
					(dy > 0) ? 's' : (dy < 0) ? 'n' : '-'
				};
				List<Component> critters = new List<Component>();
				List<Component> guards = new List<Component>();
				antagonist.Structures
					.Where(c => c.Traits.ContainsKey("Impede") && c.Traits["Impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => interaction.Interferers.Add(c));
				
				antagonist.Fauna
					.Where(c => c.Traits.ContainsKey("Impede") && c.Traits["Impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => critters.Add(c));
				if (critters.Count>0)
				{
					Constants.Randomise(critters);
					(interaction.Interferers as List<Component>).AddRange(critters);
				}
				
				antagonist.Inhabitants
					.Where(c => c.Traits.ContainsKey("Impede") && c.Traits["Impede"].Flavour.IndexOfAny(impede) > -1).ToList()
					.ForEach(c => guards.Add(c));
				if (guards.Count>0)
				{
					Constants.Randomise(guards);
					(interaction.Interferers as List<Component>).AddRange(guards);
				}
				
			}
			else interaction.Failure("Invalid destination", true);
			
			return interaction;
		}
	}
}
