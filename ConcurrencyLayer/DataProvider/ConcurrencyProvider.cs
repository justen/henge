using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Config;

using ConcurrencyLayer.Entities;


namespace ConcurrencyLayer
{
	public class ConcurrencyDataProvider : IDisposable
	{
		private IObjectContainer container	= null;
		private PersistenceCache cache		= null;
		

		public bool Initialise(string connectionString, int activationDepth)
		{
			if (this.container == null)
			{
				IConfiguration config = Db4oFactory.Configure();
				config.UpdateDepth(1);
				config.ActivationDepth(activationDepth);
				
				this.container	= Db4oFactory.OpenFile(config, connectionString);
				this.cache		= new PersistenceCache(this.container);
			
				return true;
			}
			
			return false;
		}
		
		
		public void Dispose()
		{
			if (this.container != null) this.container.Close();
			
			this.container	= null;
			this.cache		= null;
		}

		
		public T Store<T>(T entity) where T : class
		{
			return this.cache.GetPersistent(typeof(T), entity) as T;
		}
		
		
		public bool Delete(object entity)
		{
			/*IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Delete(entity);
				container.Commit();
				
				return true;
			}*/
			
			return false;
		}
		
		
		public bool Delete<T>(IList<T> entities)
		{
			/*IObjectContainer container = this.GetContainer();
			
			if (container != null && entities != null)
			{
				foreach (T entity in entities) container.Delete(entity);
				container.Commit();
				
				return true;
			}*/
			
			return false;
		}
		
		
		public IQueryable<T> Query<T>()
		{
			return InterceptingProvider.Intercept<T>(this.container.AsQueryable<T>(), this.cache);
		}
		
		
		public T Get<T>(Func<T, bool> expression) where T : class
		{
			return this.cache.GetPersistent(typeof(T), this.container.AsQueryable<T>().SingleOrDefault(expression)) as T;
		}
		
		
		public bool Lock(params object [] entities)
		{
			bool result = true;
			
			lock(this)
			{
				foreach (object entity in entities)
				{
					IPersistence persistent = entity as IPersistence;
					
					if (persistent != null)
					{
						if (!(persistent.GetBase() as PersistentBase).Lock())
						{
							result = false;
							break;
						}
					}
					else throw new Exception("Attempted to lock a transient object");
				}
			}
			
			// Couldn't lock all objects so unlock any that have been
			if (!result) this.Unlock(entities);
			
			return result;
		}
		
		
		public void Unlock(params object [] entities)
		{
			foreach (object entity in entities)
			{
				IPersistence persistent = entity as IPersistence;
				
				if (persistent != null) (persistent.GetBase() as PersistentBase).Unlock();
				else 					throw new Exception("Attempted to unlock a transient object");
			}
		}
		

		public void Flush()
		{
			/*IObjectContainer container = this.GetContainer();
			if (container != null) container.Commit();*/
		}
	}

}

