using System;


namespace Henge.Data.Entities
{
	public class Condition : ObjectEntity
	{			
		public virtual string ConditionType 	{get; set;}		
		public virtual string ConditionString 	{get; set;}
	}
}
