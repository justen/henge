using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Move
{
	public class GenericMovement : HengeRule, IAntagonist
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
		
		protected new bool Validate (HengeInteraction interaction)
		{
			bool result = false;
			if (base.Validate(interaction))
			{
				if ( interaction.Antagonist is Location && interaction.Protagonist.Location.Map == (interaction.Antagonist as Location).Map)
				{
					result = true;	
				}
				else
				{
					interaction.Failure("Invalid movement", true);
					result = false;
				}
			}
			return result;
		}
		
		protected virtual HengeInteraction AddGuards(HengeInteraction interaction)
		{
			Location antagonist = interaction.Antagonist as Location;
			Actor protagonist	= interaction.Protagonist;
			
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
				
			return interaction;
		}
		
		protected virtual HengeInteraction AddTransition(HengeInteraction interaction)
		{
			int deltaZ =  (interaction.Antagonist as Location).Z - interaction.Protagonist.Location.Z;
			if (deltaZ!=0)
			{
				interaction.Arguments.Add("Transition", deltaZ);
				interaction.Interferers.Add(interaction.Antagonist);				
			}
			return interaction;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{	
			if (this.Validate(interaction))
			{
				//Location antagonist = interaction.Antagonist as Location;
				//interaction.Impedance	= antagonist.Traits.ContainsKey("Impede") ? antagonist.Traits["Impede"].Value : Constants.Impedance;
				this.Move(interaction);
				this.AddGuards(this.AddTransition(interaction));
			}
			return interaction;
		}
		
		protected void Move(HengeInteraction interaction)
		{
			Location destination = interaction.Antagonist as Location;
			double impedance = destination.Traits.ContainsKey("Impede") ? destination.Traits["Impede"].Value : Constants.Impedance;
			interaction.Actions.Add( () => {
				impedance+=interaction.Impedance;
				if (!interaction.ProtagonistCache.UseEnergy(impedance, EnergyType.Fitness))
				{
					if (impedance > interaction.ProtagonistCache.Strength * interaction.Protagonist.Traits["Energy"].Maximum)
					{
						interaction.Failure("Your chosen route seems impassable.", false);
					}
					else interaction.Failure("Exhausted, you turn back.", false);
					
				}
			});
		}
	}
}
