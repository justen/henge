using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	static class Constants
	{
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
	}
}