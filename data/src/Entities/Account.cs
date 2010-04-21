using System;


namespace Henge.Data.Entities
{
	public class Account : Entity
	{
	    public virtual string OpenId 	{get; set;}
	    public virtual bool Enabled 	{get; set;}
	    public virtual User User		{get; set;}	
	}
}