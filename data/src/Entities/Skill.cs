
using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{


	public class Skill : Ruleset
	{
		public virtual string Name 								{get; set;}
		//the rules that this skill enables
		public virtual long Level								{get; set;}
		public virtual long Maximum								{get; set;}
		//need to find out how to do these two:
		//public virtual IList <Skill> Prerequisites				{get; set;}
		//public virtual IList <Skill> Children					{get; set;}
	}
}
