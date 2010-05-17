
using System;
using Henge.Data.Entities;

namespace Henge.Rules
{
	public interface IRule
	{
		string Interaction	{get;}
		string Ruletype		{get;}
		double Priority		{get;}
		double EvaluatePriority (HengeEntity actor);
	}
}
