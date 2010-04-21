using System;


namespace Henge.Data.Entities
{
	public class Attribute : Entity
	{
	    private string Name 	{get; set;}
	    private long Maximum 	{get; set;}
	    private long Minimum 	{get; set;}
	    private long Step 		{get; set;}
	}
}