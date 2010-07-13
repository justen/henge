using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;


namespace ConcurrencyLayer
{
	internal class ConcurrencyContainer
	{
		private static int LOCK_TIMEOUT = 1;  // Milliseconds
		private static int CACHE_LIFE	= 600;	// Seconds
		
		private ContainerCache cache;
		private ReaderWriterLockSlim objectLock = new ReaderWriterLockSlim();
		
		public long Id					{ get; set; }
		public object Object			{ get; set; }	
		public bool Dirty				{ get; set; }
		public object PersistentObject	{ get; set; }
		public DateTime Access			{ get; set; }
		
		
		public ConcurrencyContainer(long id, object obj, ContainerCache cache)
		{
			this.Id		= id;
			this.Object	= obj;
			this.Dirty	= false;
			this.Access	= DateTime.Now;
			this.cache	= cache;
		}
		
		
		public bool Expired
		{
			get
			{
				return (DateTime.Now - this.Access).Seconds > CACHE_LIFE;
			}
		}
		
		
		static public ConcurrencyContainer Create<T>(long id, object obj, ContainerCache cache) where T : class
		{
			ConcurrencyContainer result = new ConcurrencyContainer(id, obj, cache);
			result.PersistentObject		= Persistence.Create<T>(result);
			
			return result;
		}
		
		
		static public ConcurrencyContainer Create(Type type, long id, object obj, ContainerCache cache)
		{
			ConcurrencyContainer result = new ConcurrencyContainer(id, obj, cache);
			result.PersistentObject		= Persistence.Create(type, result);
			
			return result;
		}
		
		
		public bool Lock()
		{
			return this.objectLock.TryEnterWriteLock(LOCK_TIMEOUT);	
		}
		
		
		public void Unlock()
		{
			if (this.objectLock.IsWriteLockHeld) this.objectLock.ExitWriteLock();
		}
		
		
		public object GetProperty(PropertyInfo property)
		{
			object result	= null;
			Type type		= property.PropertyType;
			bool writing	= this.objectLock.IsWriteLockHeld;
			
			if (type.IsClass && type != typeof(string))
			{
				if (type.IsGenericType)
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
				{
					if (!writing) this.objectLock.EnterReadLock();
						object obj = property.GetValue(this.Object, null);
					if (!writing) this.objectLock.ExitReadLock();
					
					result = this.cache.GetPersistent(type, obj);
				}
			}
			else 
			{
				if (!writing) this.objectLock.EnterReadLock();
					result = property.GetValue(this.Object, null);
				if (!writing) this.objectLock.ExitReadLock();
			}
				
			return result;
		}
		
		
		public void SetProperty(PropertyInfo property, object value)
		{	
			if (this.objectLock.IsWriteLockHeld)
			{
				Type type		= property.PropertyType;
				object actual 	= value;
				
				// DateTime/TimeStamp will also get ignored since it is a struct not a class
				if (value != null && type.IsClass && type != typeof(string))
				{
					if (type.IsGenericType)
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
					{
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
					}
				}
				
				property.SetValue(this.Object, actual, null);
				this.Dirty = true;
			}
			else throw new Exception("Attempted to modify an unlocked persistent object");
		}
	}
	
}
