using System;
using System.IO;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using NHibernate.Tool.hbm2ddl;

using Henge.Data.Entities;

namespace Henge.Data
{
	internal class HengeForeignKeyConvention : ForeignKeyConvention
	{
		protected override string GetKeyName(PropertyInfo property, Type type)
		{
			return (property == null) ? type.Name + "Id" : property.Name + "Id";
		}
	}

	public class DataProvider
	{
		private ISessionFactory sessionFactory	= null;
		private Configuration	configuration	= null;
		
		
		public bool Initialise(string storageType, string connectionString, bool web)
		{
			bool result = false;
			string context = web?"web":"thread_static";
			if (this.sessionFactory == null)
			{
				AutoPersistenceModel map = AutoMap
					.AssemblyOf<Henge.Data.Entities.Entity>()
					.IgnoreBase<HengeEntity>()
					.IgnoreBase<PhysicalEntity>()
					.IgnoreBase<MapEntity>()
					.IgnoreBase<Actor>()
					.IgnoreBase<Ruleset>()
					.IgnoreBase<Entity>()
					.Where(t => t.Namespace.EndsWith("Entities"))
					.Conventions.Setup(c => c.Add<HengeForeignKeyConvention>());
					//.Override<Role>(k => k.HasManyToMany<Meta>(x => x.Meta).LazyLoad().Table("RoleMeta"));

				
				this.configuration = Fluently.Configure()
					.Database(this.GetConfiguration(storageType, connectionString))
					.Mappings(m => m.AutoMappings.Add(map))
					.ExposeConfiguration(c => c.Properties.Add("hbm2ddl.keywords", "none"))
					.ExposeConfiguration(x => x.SetProperty("current_session_context_class", context))
					.BuildConfiguration();
					
				this.sessionFactory = this.configuration.BuildSessionFactory(); 
					
				result = true;
			}
			
			return result;
		}

		
		private IPersistenceConfigurer GetConfiguration(string storageType, string connectionString)
		{
			IPersistenceConfigurer result;
				
			switch (storageType)
			{
				case "mysql":		result = MySQLConfiguration.Standard.ConnectionString(connectionString);			break;
				case "mssql2008":	result = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);			break;
				case "mssql2005":	result = MsSqlConfiguration.MsSql2005.ConnectionString(connectionString);			break;
				case "mssql2000":	result = MsSqlConfiguration.MsSql2000.ConnectionString(connectionString);			break;
				case "mssql7":		result = MsSqlConfiguration.MsSql7.ConnectionString(connectionString);				break;
				case "mssqlce":		result = MsSqlCeConfiguration.Standard.ConnectionString(connectionString);			break;
				case "postgre":		result = PostgreSQLConfiguration.Standard.ConnectionString(connectionString);		break;
				case "postgre81":	result = PostgreSQLConfiguration.PostgreSQL81.ConnectionString(connectionString);	break;
				case "postgre82":	result = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString);	break;
				case "jetdriver":	result = JetDriverConfiguration.Standard.ConnectionString(connectionString);		break;
				case "oracle10":	result = OracleClientConfiguration.Oracle10.ConnectionString(connectionString);		break;
				case "oracle9":		result = OracleClientConfiguration.Oracle9.ConnectionString(connectionString);		break;
				case "oracle10dc":	result = OracleDataClientConfiguration.Oracle10.ConnectionString(connectionString);	break;
				case "oracle9dc":	result = OracleDataClientConfiguration.Oracle9.ConnectionString(connectionString);	break;
				case "sqlite":		result = SQLiteConfiguration.Standard.UsingFile(connectionString);					break;
				default:			result = SQLiteConfiguration.Standard.InMemory();									break;
			}
			
			return result;
		}
		
		
		public void Dispose()
		{
			if (this.sessionFactory != null)
			{
				this.sessionFactory.Close();
				this.sessionFactory.Dispose();
				this.sessionFactory = null;
			}
			
			this.configuration = null;
		}
		
		
		public bool RegisterContext()
		{	
			if (this.sessionFactory != null)
			{
				CurrentSessionContext.Bind(this.sessionFactory.OpenSession());
				return true;
			}
			
			return false;
		}
		
		
		public bool ReleaseContext()
		{
			if (this.sessionFactory != null)
			{
				CurrentSessionContext.Unbind(this.sessionFactory);
				return true;
			}
			
			return false;
		}
		
		
		public bool UpdateSchema()
		{
			if (this.configuration != null)
			{
				new SchemaUpdate(this.configuration).Execute(true, true);
				return true;
			}
			
			return false;
		}
		
		
		public bool Update(Entity entity)
		{
			ISession session = this.GetSession();
			
			if ((session != null)&&(entity!=null))
			{
				session.SaveOrUpdate(entity);
				session.Flush();
				
				return true;
			}
			return false;
		}
		
		public bool UpdateAndRefresh(Entity entity)
		{
			ISession session = this.GetSession();
			
			if ((session != null)&&(entity!=null))
			{
				session.SaveOrUpdate(entity);
				session.Flush();
				session.Refresh(entity);
				return true;
			}
			return false;			
		}
		

		public T Get<T>(object id) where T: Entity
		{
			/*T result = default(T);
			
			//if (result is Entity)
			//{
				ISession session = this.GetSession();
				
				if (session != null) result = session.Get<T>(id);
			//}
			
			return result;*/
			ISession session = this.GetSession();
			return (session!= null) ? session.Get<T>(id) : default(T);
		}
		
		public void Refresh (Entity entity)
		{
			ISession session = this.GetSession();
			if ((session!=null)&&(entity!=null))
			{
				session.Refresh(entity);
			}
			
		}
		
		public void Delete (Entity entity)
		{
			ISession session = this.GetSession();
			if ((session!=null)&&(entity!=null))
			{
				session.Delete(entity);
				session.Flush();
			}
		}
		
		public ICriteria CreateCriteria<T>() where T : Entity
		{
			ICriteria result = null;
			
	//		if (typeof(T) is Entity)
	//		{
				ISession session = this.GetSession();
				if (session!=null) 
				{
					result = session.CreateCriteria<T>();
				}
	//		}
			return result;
		}
		
		public void Flush()
		{
			if (this.sessionFactory != null) this.sessionFactory.GetCurrentSession().Flush();
		}
		
		
		private ISession GetSession()
		{
			return (this.sessionFactory == null) ? null : this.sessionFactory.GetCurrentSession();
		}
	}

}