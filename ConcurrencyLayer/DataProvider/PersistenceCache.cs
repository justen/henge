using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Db4objects.Db4o;


namespace ConcurrencyLayer
{
	internal class PersistenceCache
	{
		private IObjectContainer container;
		private IDictionary<long, PersistentBase> cache = new SortedDictionary<long, PersistentBase>();
		
		
		public PersistenceCache(IObjectContainer container)
		{
			this.container = container;
		}
		
		
		public PersistentContainer GetContainer<T>(T item) where T : class
		{
			return this.GetBase(typeof(T), item) as PersistentContainer;
		}
		
		
		public PersistentBase GetBase(Type type, object item)
		{
			PersistentBase result = null;
			
			if (item != null)
			{
				long id = this.container.Ext().GetID(item);
				
				if (id == 0)
				{
					// If db4o is not aware of this item then store it
					this.container.Store(item);
					id = this.container.Ext().GetID(item);
				}
				
				lock (this)
				{
					if (this.cache.ContainsKey(id))	
					{
						result			= this.cache[id];
						result.Access	= DateTime.Now;
					}
					else 
					{
						if (type.IsGenericType)
						{
							//Type generic = type.GetGenericTypeDefinition();
							
							//if (generic == typeof(IList<>))				result = new PersistentList(
							//else if (generic == typeof(IDictionary<,>))	result = new PersistentDictionary(...);
						}
						else this.cache.Add(id, result = new PersistentContainer(type, id, this.Activate(item), this));
					}
				}
			}
			
			return result;	
		}
		
	
		public object GetPersistent(Type type, object item)
		{
			PersistentBase pb = this.GetBase(type, item);
			
			return pb != null ? pb.PersistentObject : null;
		}
		
		
		public object GetSource(Type type, object item)
		{
			PersistentBase pb = this.GetBase(type, item);
			
			return pb != null ? pb.Object : null;
		}
		
		
		public object Activate(object item)
		{
			if (!this.container.Ext().IsActive(item))
			{
				Console.WriteLine("  Lazy loading:" + item.ToString());
				this.container.Activate(item, 1);
			}
			
			return item;
		}
		
		
		public void Flush()
		{
			// Flush dirty items to db4o
			// Check for objects that have not been accessed for some time and remove them from the cache so that they can be garbage collected	
			List<PersistentBase> flush	= new List<PersistentBase>();
			List<long> delete			= new List<long>();
			
			lock(this)
			{
				foreach (KeyValuePair<long, PersistentBase> kvp in this.cache)
				{
					if (kvp.Value.Dirty)	flush.Add(kvp.Value);
					if (kvp.Value.Expired)	delete.Add(kvp.Key);
				}
				
				foreach (long id in delete) this.cache.Remove(id);
			}
			
			foreach (PersistentBase item in flush)
			{
				item.Lock();
					this.container.Store(item.Object);
					item.Dirty = false;
				item.Unlock();
			}
		}
	}
}
