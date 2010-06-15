
using System;
using Henge.Data.Entities;

namespace Henge.Rules
{


	static class Common
	{
		//maximum skill level
		public const double SkillMax = 1.0;
		//maximum energy gain per second
		public const double MaxEnergyGain = 1.0;
		//maximum energy
		public const double MaxEnergy = 10.0; 
		//skill gain rate
		public const double SkillAcquisition = 0.01;
		//How close you need to be to passing a (failed) skill check to get a little bit of skill anyway
		public const double AlmostPassed = 0.01;
		//How much skill you get for passing a barely-failed skill check
		public const double CommiserationPrize = 0.001;
		//Value of a newly-granted child skill
		public const double SkillGrantDefault = 0.1;
		//base impedance (for movement)
		public const double Impedance = 1.0;
		//Default skill for use when absence of a skill shouldn't be 0.
		public const double DefaultSkill = 0.1;
		//Base weight for actors unless otherwise specified
		public const double ActorBaseWeight = 70.0;
		//Weight-to-impedance transform
		public const double WeightToImpedance = 0.00143;

		
		//Performs a SkillCheck. actor is the entity whose skill is being tested, name is the name of the skill and difficulty is 
		//the difficulty of the task (and should be in the range 0.0-1.0. If the test is passed the function returns true and increases
		//the skill of the actor. If the test is failed, the function returns false. If it's only just failed, the actor gets a small
		//skill increase.
		public static bool SkillCheck(Actor actor, string name, double difficulty)
		{
			bool result = false;
			if ( difficulty<0 ) difficulty = 0;
			if ( actor.Skills.ContainsKey(name) )
			{
				Skill skill = actor.Skills[name];
				if (skill.Value >= difficulty)
				{
					if (skill.Value == 0) skill.Value = Common.SkillAcquisition * Common.AlmostPassed;
					else 
					{
						if (skill.Add(Common.SkillAcquisition * difficulty/(skill.Value))==1.0)
						{
							foreach (string newSkill in skill.Children)
							{
								if (!actor.Skills.ContainsKey(newSkill))
								{
									actor.Skills.Add(newSkill, new Skill(){Value = Common.SkillGrantDefault});
								}
							}
						}
					}
					result = true;	
				}
				else
				{
					double miss = difficulty - skill.Value;
					if (miss < Common.AlmostPassed)
					{
						skill.Add(Common.CommiserationPrize * (Common.AlmostPassed-miss)/Common.AlmostPassed );
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
				energy.Transfer(actor.Traits["reserve"], Common.MaxEnergyGain * actor.Skills["fitness"].Value * ts.Seconds);
			}
			return energy;
		}
	}
}
