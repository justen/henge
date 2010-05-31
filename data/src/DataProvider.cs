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
		private IContext context		= null;
		private IObjectServer server	= null;
		
		
		//public bool Initialise(string storageType, string connectionString, bool web)
		public bool Initialise(string connectionString, bool web)
		{
			if (this.server == null)
			{
				IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
				config.Common.Add(new TransparentPersistenceSupport());
				config.Common.Add(new TransparentActivationSupport());
				
				//config.Common.IndexClass<User>();
				
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
		
		
		public void Bootstrap()
		{
			if (this.server != null)
			{
				IObjectContainer container = this.server.OpenClient();
				
				var all = from Entity a in container select a;
				foreach (var e in all) container.Delete(e);
				
				Appearance ap				= new Appearance { Priority = 1, Name = "Nondescript Wasteland", Description = "looks like the Creator simply couldn''t be bothered to do anything with it. It is totally unremarkable in every way", ShortDescription = "a thoroughly boring spot" };
				Henge.Data.Entities.User u	= new Henge.Data.Entities.User { Name = "test", Password = "A94A8FE5CCB19BA61C4C0873D391E987982FBBD3", Clan = "Test" };
				Map m 						= new Map { Name = "Main" };
				Location l					= new Location { X = 0, Y = 0, Map = m, BaseAppearance = ap };
				Avatar av					= new Avatar { Name = "Og", User = u, Location = l, BaseAppearance = new Appearance { Name = "Og" } };
				
				u.Avatars.Add(av);
				m.Locations.Add(l);
				l.Inhabitants.Add(av);
				
				for (int y = -2; y < 3; y++)
				{
					for (int x = -2; x < 3; x++) 
					{
						if (x == 0 && y == 0) continue;
						m.Locations.Add(new Location { X = x, Y = y, Map = m, BaseAppearance = ap });
					}
				}
				
				container.Store(u);
				container.Store(m);
				
				container.Commit();
				container.Close();
				container.Dispose();
			}
			
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
		
		
		public IQueryable<T> Query<T>() where T : Entity
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