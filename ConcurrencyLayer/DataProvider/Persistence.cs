using System;
using System.Linq;
using System.Reflection;

using Castle.Core.Interceptor;
using Castle.DynamicProxy;


namespace ConcurrencyLayer
{
	internal class ReflectHelper
	{
		public static bool IsGetter(MethodInfo method)
		{
			return method.IsSpecialName && method.Name.StartsWith("get_");
		}
		
		public static bool IsSetter(MethodInfo method)
		{
			return method.IsSpecialName && method.Name.StartsWith("set_");
		}
		
		public static string GetPropertyName(MethodInfo method)
		{
			return method.Name.Substring(4);
		}
	}
	
	
	public interface IPersistence
	{
		object GetSource();
		object GetContainer();
	}
	
	
	internal class Persistence
	{
		private static readonly ProxyGenerator generator		= new ProxyGenerator();
		private static readonly ProxyGenerationOptions options	= new ProxyGenerationOptions(new PersistentProxyGeneration());
		private static readonly Type [] interfaces				= new Type [] { typeof(IPersistence) };
		
		
		public static T Create<T>(ConcurrencyContainer container) where T : class
		{
			return Create(typeof(T), container) as T;
		}
		
		
		public static object Create(Type type, ConcurrencyContainer container)
		{
			//return generator.CreateClassProxy(type, options, new PersistentInterceptor(container));
			return generator.CreateClassProxy(type, interfaces, options, new PersistentInterceptor(container));
		}
		
		
		/*public static bool IsPersistent(object value)
		{
			return (value as IPersistence) != null;
		}*/
		
		
		/*private static T Clone<T>(ConcurrencyContainer container, T dst, PersistentInterceptor interceptor) where T : class
		{
			T src 						= container.Object as T;
			PropertyInfo [] properties 	= typeof(T).GetProperties();
			
			container.ObjectLock.EnterReadLock();
				interceptor.Version = container.Version;
			
				foreach (PropertyInfo property in properties)
				{
					Type type = property.PropertyType;
				
					if (type.IsPrimitive || type.IsValueType || type == typeof(string) || type == typeof(DateTime)) 
					{
						property.SetValue(dst, property.GetValue(src, null), null);
					}
				}
			container.ObjectLock.ExitReadLock();
			
			interceptor.Active = true;
			
			return dst;
		}*/
	}
	
	
	internal class PersistentInterceptor : IInterceptor
	{
		private ConcurrencyContainer container;
		
		
		public PersistentInterceptor(ConcurrencyContainer container)
		{
			this.container	= container;
		}
		
		
		public void Intercept(IInvocation invocation)
		{
			PropertyInfo property = invocation.TargetType.GetProperty(ReflectHelper.GetPropertyName(invocation.Method));
			
			if (property != null)
			{
				if (ReflectHelper.IsGetter(invocation.Method))	invocation.ReturnValue = this.container.GetProperty(property);
				else 											this.container.SetProperty(property, invocation.Arguments[0]);
			}
			else switch (invocation.Method.Name)
			{
				case "GetSource": 			invocation.ReturnValue = this.container.Object;	break;
				case "GetContainer":		invocation.ReturnValue = this.container;		break;
			}

			//invocation.Proceed();
		}
	}
	
	
	internal class PersistentProxyGeneration : IProxyGenerationHook
	{
		public bool ShouldInterceptMethod(Type type, MethodInfo method)
		{
			return ReflectHelper.IsGetter(method) || ReflectHelper.IsSetter(method) || method.Name == "GetSource" || method.Name == "GetContainer";	
		}
		
		
		public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
		{
		}
		
		
		public void MethodsInspected()
		{
				
		}
	}
}
		