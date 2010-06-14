
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
		//skill gain rate
		public static double skillAcquisition = 0.01;
		//How close you need to be to passing a (failed) skill check to get a little bit of skill anyway
		public static double almostPassed = 0.01;
		//How much skill you get for passing a barely-failed skill check
		public static double comiserationPrize = 0.001;
		//Value of a newly-granted child skill
		public static double skillGrantDefault = 0.1;

		
		//Performs a SkillCheck. actor is the entity whose skill is being tested, name is the name of the skill and difficulty is 
		//the difficulty of the task (and must be in the range 0.0-1.0. If the test is passed the function returns true and increases
		//the skill of the actor. If the test is failed, the function returns false. If it's only just failed, the actor gets a small
		//skill increase.
		public static bool SkillCheck(Actor actor, string name, double difficulty)
		{
			bool result = false;
			if ((0.0<=difficulty) && (difficulty<=1.0))
			{
				if (!actor.Skills.ContainsKey(name)) actor.Skills.Add(name, new Skill(){Value = 0});
				Skill skill = actor.Skills[name];
				if (skill.Value >= difficulty)
				{
					if (skill.Value == 0) skill.Value = Common.skillAcquisition * Common.almostPassed;
					else 
					{
						if (skill.Add(Common.skillAcquisition * difficulty/(skill.Value))==1.0)
						{
							foreach (string newSkill in skill.Children)
							{
								if (!actor.Skills.ContainsKey(newSkill))
								{
									actor.Skills.Add(newSkill, new Skill(){Value = Common.skillGrantDefault});
								}
							}
						}
					}
					result = true;	
				}
				else
				{
					double miss = difficulty - skill.Value;
					if (miss < Common.almostPassed)
					{
						skill.Add(comiserationPrize * (Common.almostPassed-miss)/Common.almostPassed );
					}
				}
			}
			return result;
		}
		
		
		//attempt to use the specified amount of energy of the actor. Return false if the actor is already at zero energy or less,
		//or if the amount of energy to be used is greater than the actors strength skill.
		public static bool UseEnergy(Actor actor, double amount)
		{
			Trait energy = Common.GetEnergy(actor);
			if  ( energy.Value > 0 )  
			{
				if ( Common.SkillCheck(actor, "strength",  amount/energy.Maximum) )
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
			if ( actor.Skills.ContainsKey("fitness") && (actor.Skills["fitness"].Value>0) )
			{
				TimeSpan ts = DateTime.Now - actor.LastModified;
				energy.Transfer(actor.Traits["reserve"], Common.maxEnergyGain * actor.Skills["fitness"].Value * ts.Seconds);
			}
			return energy;
		}
	}
}
