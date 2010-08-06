using System;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public abstract class HengeModifier : IModifier
	{
		protected DataProvider db = null;
		
		
		public abstract string Target { get; }
		
		public void Initialise(DataProvider db)
		{
			this.db = db;
		}
		
		
		public abstract Trait Apply(Actor actor);
	}
}