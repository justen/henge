using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Move
{
	public class BasicMove : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			return subject is Actor;
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Moving; become more obvious:
			subject = interaction.Protagonist;
			return (Constants.StandardVisibility * interaction.ProtagonistCache.Conspicuousness);
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			// structure of this rule is
			//
			//  IF (ConditionsMet(protagonist, antagonists, interaction))
			//  THEN ApplyChanges (protagonist, antagonists, interaction)
			//
			if (!interaction.Finished)
			{
				Location antagonist	= interaction.Antagonist as Location;
				Location source		= interaction.Protagonist.Location;
				
				if (interaction.Protagonist != null && antagonist != null)
				{
					if (this.CalculateDistance(source, antagonist) <= 2)
					{
						//For standard movement, we should only be able to move up and down a limited range of slopes
						double gradient = (double)(antagonist.Z - source.Z)/Constants.MaximumMoveZ;
						interaction.Log+=(string.Format("Gradient: {0}", gradient));
						if (gradient != 0)
						{
							if (gradient > 0) 
							{
								switch(interaction.ProtagonistCache.SkillCheck("Climb", gradient, gradient * 0.25 * interaction.Impedance, gradient * interaction.Impedance, EnergyType.Strength  ))
								{
								case SkillResult.PassExhausted: 	interaction.Failure( "The ascent is punishing. You are forced to give up.", false);
																	break;
								case SkillResult.PassSufficient: 	interaction.Log += "The ascent is punishing. You scramble up as best you can. ";
																	break;
								case SkillResult.FailExhausted: 	interaction.Failure( "The ascent is punishing. You are forced to give up.", false);
																	break;
								case SkillResult.FailSufficient: 	interaction.Log += "The ascent is punishing. You scramble up as best you can, trying not to think of how you're going to get down again. ";
																	break;
								}
							}
							else
							{
								gradient = -gradient;
								SkillResult check = interaction.ProtagonistCache.SkillCheck("Climb", gradient, 0, 0, EnergyType.None  );
								
								if (check == SkillResult.PassSufficient)
								{
									//you're comfortable charging down this slope - impedance will drop
									//as a result (max reduction is to halve the impedance)
									interaction.Impedance-=gradient * interaction.Impedance;
								}
								else
								{
									//You're going to have to climb down this
									
									switch (interaction.ProtagonistCache.SkillCheck("Climb", gradient * 0.75, gradient * interaction.Impedance, 0, EnergyType.Strength))
									{
										case SkillResult.PassSufficient: interaction.Log += "The descent is difficult; you scramble down carefully. ";
																		 break;
										case SkillResult.PassExhausted: interaction.Failure("The precipitous descent is exhausting; you are forced to give up", false);
																		 break;
										default: interaction.Failure("The descent is too intimidating for you to attempt", false); break;
									}
												
								}
							}
						}
						if (!interaction.Finished)
						{
							//interaction.Log+=string.Format("Total move cost: {0} ", interaction.Impedance);
							if (interaction.ProtagonistCache.UseEnergy(interaction.Impedance, EnergyType.Fitness))
							{
								this.ApplyInteraction(interaction, interaction.Protagonist, antagonist);
								// Now everything that was trying to impede progress is going to have to take damage I suppose...?
							}
							else 
							{
								if (interaction.Impedance > interaction.ProtagonistCache.Strength * interaction.Protagonist.Traits["Energy"].Maximum)
								{
									interaction.Failure(string.Format("{0}Your chosen route seems impassable.", interaction.Log), false);
								}
								else interaction.Failure(string.Format("{0}You are unable to summon sufficient energy to make it to your destination.", interaction.Log), false);
							}
						}
					}
					else interaction.Failure("You cannot move that far", true);
				}
			}
	
			return interaction;
		}	
		
		
		private int CalculateDistance(Location source, Location destination)
		{
			int deltaX = source.X - destination.X;
			int deltaY = source.Y - destination.Y;
			// currently can't run in z, so don't bother calculating it.
			// int deltaZ = source.Coordinates.Z - destination.Coordinates.Z;
			
			return deltaX * deltaX + deltaY * deltaY;
		}
		
		
		private void ApplyInteraction (HengeInteraction interaction, Actor actor, Location target)
		{
			// We would apply any charges built up in interaction here, but there are none at present so
			// it's not going to be used.
			if (actor is Avatar)
			{
				Avatar avatar = actor as Avatar;
				
				using (interaction.Lock(avatar, avatar.Location.Inhabitants, target.Inhabitants))
				{
					avatar.Location.Inhabitants.Remove(avatar);
					target.Inhabitants.Add(avatar);
					avatar.Location = target;
				}
					
				interaction.Success(string.Format("{0}You reach your destination, a {1}", interaction.Log, target.Inspect(avatar).ShortDescription));	
			}
			else
			{
				Npc npc = actor as Npc;
				
				using (interaction.Lock(npc, npc.Location.Fauna, target.Fauna))
				{
					npc.Location.Fauna.Remove(npc);
					target.Fauna.Add(npc);
					npc.Location = target;
				}
					
				interaction.Success("Moved");	
			}
		}	
	}
}
