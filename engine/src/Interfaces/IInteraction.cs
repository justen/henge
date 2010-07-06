using System;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IInteraction
	{
		Component Subject				{ get; }
		Component Antagonist			{ get; set; }
		Actor Protagonist				{ get; set; }
		IList<Func<bool, bool>> Deltas 	{ get; }
		IList<Component> Interferers	{ get; }
		string Conclusion				{ get; }
		bool Finished					{ get; }
		bool Succeeded					{ get; }
		bool Illegal					{ get; }
		
		IInteraction Conclude();
		IInteraction Success(string message);
		IInteraction Failure(string message, bool illegal);
		void SetSubject(Component subject);
	}
}
