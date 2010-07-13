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
		
		
		public object GetProperty(PropertyInfo property)
		{
			object result	= null;
			Type type		= property.PropertyType;
			
			if (type.IsClass && type != typeof(string))
			{
				/*if (type.IsGenericType)
				{
					Type generic = type.GetGenericTypeDefinition();
					
					if (generic == typeof(IList<>))
					{
						Console.WriteLine("  Intercepted list access");
					}
					else if (generic == typeof(IDictionary<,>))
					{
						Console.WriteLine("  Intercepted dictionary access");
					}
				}
				else
				{*/
					if (!this.Writing) this.objectLock.EnterReadLock();
						object obj = property.GetValue(this.Object, null);
					if (!this.Writing) this.objectLock.ExitReadLock();
					
					result = this.cache.GetPersistent(type, obj);
				//}
			}
			else 
			{
				if (!this.Writing) this.objectLock.EnterReadLock();
					result = property.GetValue(this.Object, null);
				if (!this.Writing) this.objectLock.ExitReadLock();
			}
				
			return result;
		}
		
		
		public void SetProperty(PropertyInfo property, object value)
		{	
			this.AssertWrite();

			Type type		= property.PropertyType;
			object actual 	= value;
			
			// DateTime/TimeStamp will also get ignored since it is a struct not a class
			if (value != null && type.IsClass && type != typeof(string))
			{
				/*if (type.IsGenericType)
				{
					Type generic = type.GetGenericTypeDefinition();
						
					if (generic == typeof(IList<>))
					{
						Console.WriteLine("  Intercepted list assignment");
					}
					else if (generic == typeof(IDictionary<,>))
					{
						Console.WriteLine("  Intercepted dictionary assignment");
					}
				}
				else
				{*/
					IPersistence persistent = value as IPersistence;
					
					if (persistent == null)
					{
						Console.WriteLine("  Intercepted entity assignment - target is not persistent");
						
						// This should create a new container and actually store the object to the database
						actual = this.cache.GetSource(type, value);
					}
					else
					{
						Console.WriteLine("  Intercepted entity assignment - target is persistent");
						
						actual = persistent.GetSource();
					} 
				//}
			}
			
			property.SetValue(this.Object, actual, null);
			this.Dirty = true;
		}
	}
	
}
