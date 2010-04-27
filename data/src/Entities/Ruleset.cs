
using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{


	public abstract class Ruleset : Entity
	{
		public virtual IList<AttributeModifier> modifiers 		{get; set;}
		public virtual IList<RuleDefinition> ProtagonistRules	{get; set;}
		public virtual IList<RuleDefinition> AntagonistRules	{get; set;}
		public virtual IList<RuleDefinition> InterferenceRules	{get; set;}
	}
}
