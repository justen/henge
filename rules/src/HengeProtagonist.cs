
using System;
using Henge.Data.Entities;


namespace Henge.Rules
{


	public abstract class HengeProtagonist : ProtagonistRule
	{

		public HengeProtagonist ()
		{
		}

		//attempt to use the specified amount of energy of the actor. Return false if the actor didn't have enough energy.
		//if overdraw = true, the actor can use more energy than it has BUT cannot use any energy if it is already negative
		protected bool UseEnergy(Actor actor, long energy, bool overdraw)
		{
			this.FastTick(actor);
			if (overdraw || ( energy <= actor.Attributes["energy"] ))
			{
				if ( actor.Attributes["energy"]>0)
				{
					actor.Attributes["energy"] -= energy;
					return true;
				}
			}
			return false;
		}
		
		//charge up the actor with the specified amount of energy. Returns the amount of energy actually added to the actor
		protected void FastTick(Actor actor)
		{
			if (actor.Attributes.ContainsKey("maxEnergy")==false)
			{
				actor.Attributes.Add("maxEnergy", 0);	
			}
			if (actor.Attributes.ContainsKey("energy")==false)
			{
				actor.Attributes.Add("energy", 0);
			}
			else
			{
				//check whether the actor is actually capable of gaining energy
				if (actor.Attributes.ContainsKey("fitness") && (actor.Attributes["fitness"]>0))
				{
					TimeSpan ts = DateTime.Now - actor.LastModified;
					long ticks = ((long)ts.TotalSeconds/Constants.fastTick);
					if (ticks>0)
					{
						actor.LastModified.AddSeconds(ticks * Constants.fastTick);
						actor.Attributes["energy"] +=  ticks * (actor.Attributes["fitness"] /Constants.skillMax);
						if (actor.Attributes["energy"] > actor.Attributes["maxEnergy"]) actor.Attributes["energy"] = actor.Attributes["maxEnergy"];
					}
				}
			}
		}
		
		//returns the current energy level of the actor
		protected long GetEnergy (Actor actor)
		{
			this.FastTick(actor);
			return actor.Attributes["energy"];
		}
	}
}
