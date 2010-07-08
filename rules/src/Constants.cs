using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	static class Constants
	{
		public static class Tick
		{
			public const double MetabolicRate = 1.0;
			public static class Healthy
			{
				public const double Heal = 1.0;
				public const double Revitalise = 2.0;
			}
			public static class Ill
			{
				public const double Heal = -1.0;
				public const double Revitalise = 1.0;
			}
		}
		
		
		// Maximum skill level
		public const double SkillMax = 1.0;
		// Maximum energy gain per second
		public const double MaxEnergyGain = 1.0;
		// Maximum energy
		public const double MaxEnergy = 10.0; 
		// Skill gain rate
		public const double SkillAcquisition = 0.01;
		// How close you need to be to passing a (failed) skill check to get a little bit of skill anyway
		public const double AlmostPassed = 0.01;
		// How much skill you get for passing a barely-failed skill check
		public const double CommiserationPrize = 0.001;
		// Value of a newly-granted child skill
		public const double SkillGrantDefault = 0.1;
		// Base impedance (for movement)
		public const double Impedance = 1.0;
		// Default skill for use when absence of a skill shouldn't be 0.
		public const double DefaultSkill = 0.1;
		// Base weight for actors unless otherwise specified
		public const double ActorBaseWeight = 70.0;
		// Weight-to-impedance transform
		public const double WeightToImpedance = 0.00143;
		// Weight-to-strength for lifting skill checks
		public const double WeightToLiftStrength = 0.005;
		// Standard visibility of things which aren't either hidden or deliberately conspicuous
		// things that have a visibility of less than this may be invisible to some characters
		public const double StandardVisibility = 0.5;
		
		public const double HighVisibility = 0.75;
		
		public const double BaseConspicuousness = 1.0;
		
		public const double SearchCost = 1.0;
		
		public const double SearchDifficulty = 2.0;
		
		public const double ScoutCost = 2.5;
		
		public const double LocationScouting = 0.1;
		public const double EdificeScouting = 0.2;
		public const double ActorScouting = 0.3;
		
		public const double DefaultCover = 1;
		
		public const double InspectionCharge = 1;
		
		public const double TalkCost = 0.5;
		
		
		
		
		private static Random rng = new Random();
		
		public static double RandomNumber
		{
			get
			{
				return Constants.rng.NextDouble();
			}
		}
		
		
		public static void Randomise(IList<Component> list)
		{
			int swap, count = list.Count;
			Component temp;
			
		    for (int i=0; i<count; i++)
		    {
		        swap = Constants.rng.Next(i, count);
				
		        if (swap != i)
		        {
		            temp		= list[i];
		            list[i]		= list[swap];
		            list[swap]	= temp;
		        }
		    }
		}
	}
}
