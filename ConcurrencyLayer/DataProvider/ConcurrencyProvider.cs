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
		//private IDictionary<long, WeakReference> cache 	= new SortedDictionary<long, WeakReference>();
		private ContainerCache cache		= null;
		

		public bool Initialise(string connectionString, int activationDepth)
		{
			if (this.container == null)
			{
				IConfiguration config = Db4oFactory.Configure();
				config.UpdateDepth(1);
				config.ActivationDepth(activationDepth);
				
				this.container	= Db4oFactory.OpenFile(config, connectionString);
				this.cache		= new ContainerCache(this.container);
			
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

		
		public T Store<T>(T entity)
		{
			/*IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Store(entity);
				container.Commit();
				
				return entity;
			}	*/
			
			// Temporary store code - should be using the queues
			this.container.Store(entity);
			this.container.Commit();
			
			return entity;
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
			/*IObjectContainer container = this.GetContainer();
			
			return (container == null) ? null : container.AsQueryable<T>(); //(from T item in container select item).AsQueryable();*/
			
			return null;
		}
		
		
		public T Get<T>(Func<T, bool> expression) where T : class
		{
			ConcurrencyContainer container 	= this.cache.GetContainer<T>(this.container.AsQueryable<T>().SingleOrDefault(expression));
			
			return (container != null) ? container.PersistentObject as T : null;
		}
		

		public void Flush()
		{
			/*IObjectContainer container = this.GetContainer();
			if (container != null) container.Commit();*/
		}
	}

}

