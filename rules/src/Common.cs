
using System;
using Henge.Data.Entities;

namespace Henge.Rules
{


	static class Common
	{
		//maximum skill level
		public static double skillMax = 1.0;
		//maximum energy gain per second
		public static double maxEnergyGain = 1.0;
		//maximum energy
		public static double maxEnergy = 10.0; 
		
		//attempt to use the specified amount of energy of the actor. Return false if the actor didn't have enough energy.
		//if overdraw = true, the actor can use more energy than it has BUT cannot use any energy if it is already negative
		public static bool UseEnergy(Actor actor, long amount)
		{
			Trait energy = Common.GetEnergy(actor);
			if  ( energy.Value > 0 )  
			{
				Trait strength  = actor.Traits["strength"];
				if ( amount <= strength.Value * energy.Maximum) 
				{
					energy.Value -= amount;
					return true;
				}
			}
			return false;
		}
		
		//returns the current energy level of the actor
		public static Trait GetEnergy (Actor actor)
		{
			Trait energy = actor.Traits["energy"];
			//check whether the actor is actually capable of gaining energy
			if ( actor.Skills.ContainsKey("fitness") && (actor.Skills["fitness"]>0) )
			{
				TimeSpan ts = DateTime.Now - actor.LastModified;
				energy.Transfer(actor.Traits["reserve"], Common.maxEnergyGain * actor.Skills["fitness"] * ts.Seconds);
			}
			return energy;
		}
	}
}
