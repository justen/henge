using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Config;

using Henge.Data.Context;
using Henge.Data.Entities;


namespace Henge.Data
{
	public class DataProvider
	{
		private IContext context			= null;
		private IObjectServer server		= null;
		
		
		//public bool Initialise(string storageType, string connectionString, bool web)
		public bool Initialise(string connectionString, bool web)
		{
			if (this.server == null)
			{
				IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
				config.Common.Add(new TransparentPersistenceSupport());
				config.Common.Add(new TransparentActivationSupport());
				
				//config.Common.IndexClass<Entity>();
				
				this.context	= web ? (IContext)new WebContext() : (IContext)new ThreadContext();
				this.server		= Db4oClientServer.OpenServer(config, connectionString, 0);
			
				return true;
			}
			
			return false;
		}
		
		
		public void Dispose()
		{
			if (this.server != null) this.server.Close();
		}
		
		
		public bool RegisterContext()
		{	
			if (this.server != null)
			{
				this.context.Container = this.server.OpenClient();
				return true;
			}
			
			return false;
		}
		
		
		public bool ReleaseContext()
		{
			if (this.server != null && this.context.Container != null) 
			{
				this.context.Container.Commit();
				this.context.Container.Close();
				this.context.Container = null;
				return true;
			}
			
			return false;
		}
		
		
		public T Store<T>(T entity) where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Store(entity);
				container.Commit();
				
				return entity;
			}	
			
			return null;
		}
		
		
		public bool Delete(Entity entity)
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null)
			{
				container.Delete(entity);
				container.Commit();
				
				return true;
			}
			
			return false;
		}
		
		
		public bool Delete(IList<Entity> entities)
		{
			IObjectContainer container = this.GetContainer();
			
			if (container != null && entities != null)
			{
				foreach (Entity entity in entities) container.Delete(entity);
				container.Commit();
				
				return true;
			}
			
			return false;
		}
		
		
		/*public IQueryable Query
		{
			get
			{
				return this.GetContainer() as IQueryable;
			}
		}*/
		
		
		public IQueryable<T> Query<T>()
		{
			IObjectContainer container = this.GetContainer();
			
			return (container == null) ? null : (from T item in container select item).AsQueryable();
		}
		
		
		public T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : Entity
		{
			IObjectContainer container = this.GetContainer();
			
			return (container == null) ? null : (from T item in container select item).AsQueryable().SingleOrDefault(expression);
		}
		/*public IQueryable<T> Query<T>()
		{
			IObjectContainer container = this.GetContainer();
			return (container == null) ? null : (from T item in container select item).AsQueryable();
		}*/
		
		
        /*/// <summary>
        /// Returns all T records in the repository
        /// </summary>
        public IQueryable<T> All<T>() {
            return (from T items in db
                    select items).AsQueryable();
        }

        /// <summary>
        /// Finds an item using a passed-in expression lambda
        /// </summary>
        public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) {
            return All<T>().SingleOrDefault(expression);
        }*/
		
		
		public void Flush()
		{
			IObjectContainer container = this.GetContainer();
			if (container != null) container.Commit();
		}
		
		
		public bool UpdateSchema()
		{
			return false;
		}
		
		
		private IObjectContainer GetContainer()
		{
			return (this.server == null) ? null : this.context.Container;
		}
		
		
		/*public void Refresh(Entity entity)
		{
			ISession session = this.GetSession();
			
			if (session != null && entity != null) session.Refresh(entity);
		}
		*/
	}

}