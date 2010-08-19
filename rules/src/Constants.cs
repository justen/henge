using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public enum EnergyType
	{
		Strength,
		Fitness,
		Concentration,
		None
	}
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
		public const double EnergyGain = 1.0;
		// Maximum energy
		public const double MaxEnergy = 10.0; 
		//span of max energy to min energy
		public const double EnergySpan = 20.0;
		// Skill gain rate
		public const double SkillAcquisition = 0.01;
		// Multiplier for skill gain when it's one of the base skills being used within another skill check
		public const double BaseEnergyUseSkillMultiplier = 0.1;
		// How close you need to be to passing a (failed) skill check to get a little bit of skill anyway
		// This is used: almostPassed = Constants.AlmostPassed + Constants.AlmostPassed * (Constants.RandomNumber - 0.5);
		// I.e. ranges from AlmostPassed/2 to AlmostPassed * 1.5
		public const double AlmostPassed = 0.15;
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
		
		//Maximum Z-difference someone can move on a base-move action.
		public const double MaximumMoveZ = 120;
		
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
		
		public const double StartingSkill = 0.5;
		
		//Default climb difficulty - makes climbing a tiny bit harder
		//This is only really used when the climb difficulty trait hasn't been set for an edifice.
		public const double DefaultClimbDifficulty = 1.01;
		
		public const double MaxMovementDifficulty = 255;
		public const double BaseTrack = 50.0;
		public static readonly TimeSpan TraceLife = new TimeSpan(0, 5, 0);
		
		public const int MaximumTracks = 25;
		public const double MaximumTrackValue = 100.0;
		
		private static Random rng = new Random();
		
		public static double RandomNumber
		{
			get
			{
				return Constants.rng.NextDouble();
			}
		}
		
		
		public static void Randomise<T>(IList<T> list) where T : Entity
		{
			int swap, count = list.Count;
			T temp;
			
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
