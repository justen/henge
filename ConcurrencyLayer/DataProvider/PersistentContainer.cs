using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;


namespace ConcurrencyLayer
{
	internal class PersistentContainer : PersistentBase
	{
		public PersistentContainer(Type type, long id, object source, PersistenceCache cache) : base(id, source, cache)
		{
			this.PersistentObject = Persistence.Create(type, this);
		}
		
		
		private static bool RequiresPersistent(Type type)
		{
			return (type.IsClass || type.IsGenericType) && type != typeof(string);
		}
			
		
		
		public object GetProperty(PropertyInfo property)
		{
			object result	= null;
			Type type		= property.PropertyType;
			
			if (PersistentContainer.RequiresPersistent(type))
			{
				object obj	= this.Read(() => property.GetValue(this.Object, null));
				result		= this.cache.GetPersistent(type, obj);
			}
			else result = this.Read(() => property.GetValue(this.Object, null));
				
			return result;
		}
		
		
		public void SetProperty(PropertyInfo property, object value)
		{	
			this.AssertWrite();

			Type type		= property.PropertyType;
			object actual 	= value;
			
			// DateTime/TimeStamp will also get ignored since it is a struct not a class
			if (value != null && PersistentContainer.RequiresPersistent(type))
			{
				// If target is not persistent, create a new persistent wrapper and actually store the object to the database. If the target is persistent simply retrieve its source.
				actual = (value is IPersistence) ? (value as IPersistence).GetSource() : this.cache.GetSource(type, value);
			}
			
			property.SetValue(this.Object, actual, null);
		}
	}
}
