using System;

//using FluentNHibernate.Automapping;
//using FluentNHibernate.Automapping.Alterations;



namespace Henge.Data.Entities
{
	public class Statistic : Entity
	{
	    public virtual Attribute Attribute	{ get; set; }
	    public virtual long Value			{ get; set; }
		
		//public virtual HengeEntity Owner { get; set; }
	}
	
	
	/*public class StatisticMappingOverride : IAutoMappingOverride<Statistic>
	{
		public void Override(AutoMapping<Statistic> mapping)
		{
			mapping.ReferencesAny<HengeEntity>(x => x.Owner)
				.EntityTypeColumn("OwnerType")
  				.EntityIdentifierColumn("OwnerId")
				.IdentityType(x => x.Id);
				/*.AddMetaValue<Group>("Group")
				.AddMetaValue<Avatar>("Avatar")
				.AddMetaValue<Item>("Item")
				.AddMetaValue<Edifice>("Edifice")
				.AddMetaValue<Location>("Location")
				.AddMetaValue<Npc>("Npc")* /
  				//.IdentityType<long>();
				//.Cascade.All();
		}
	}*/
}