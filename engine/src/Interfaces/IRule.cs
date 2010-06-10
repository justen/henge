using System;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IRule
	{
		string Interaction	{ get; }
		double Priority(Component subject);
		Interaction Apply(Interaction interaction);
	}
}
