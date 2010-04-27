
using System;
using Henge.Engine.Ruleset;

namespace Henge.Engine.Ruleset.Core
{


	public abstract class BasicRun
	{
		protected string name = "Basic Run";
		protected string ruletype = "Run";
		public string Name
		{
		 	get
			{
				return this.name;
			}
			protected set
			{
				this.name = value;
			}
		}
		
		public string Ruletype
		{
		 	get
			{
				return this.ruletype;
			}
			protected set
			{
				this.ruletype = value;
			}
		}
	}
}
