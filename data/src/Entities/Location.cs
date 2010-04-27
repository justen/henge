using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Location : HengeEntity
	{
	    public virtual IList<Avatar> Inhabitants		{get; set;}
		public virtual IList<Actor> Stuff {get; set;}
		public virtual IList<Edifice> Structures		{get; set;}
		public virtual IList<Npc> Fauna					{get; set;}
		public virtual int X							{get; set;}			
	    public virtual int Y							{get; set;}
	    public virtual int Z							{get; set;}
	    //List of the Regions this Location is in
	    public virtual IList<Region> Regions			{get; set;}
	    //the Map this location exists in
	    public virtual Map Map							{get; set;}
	}
}