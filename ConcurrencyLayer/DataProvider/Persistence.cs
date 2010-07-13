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
		object GetBase();
	}
	
	
	internal class Persistence
	{
		private static readonly ProxyGenerator generator		= new ProxyGenerator();
		private static readonly ProxyGenerationOptions options	= new ProxyGenerationOptions(new PersistentProxyGeneration());
		private static readonly Type [] interfaces				= new Type [] { typeof(IPersistence) };
		
		
		public static T Create<T>(PersistentContainer container) where T : class
		{
			return Create(typeof(T), container) as T;
		}
		
		
		public static object Create(Type type, PersistentContainer container)
		{
			PersistentInterceptor interceptor 	= new PersistentInterceptor(container);
			object result 						= generator.CreateClassProxy(type, interfaces, options, interceptor);
			interceptor.Active					= true;
			
			return result;
		}
	}
	
	
	internal class PersistentInterceptor : IInterceptor
	{
		public bool Active { get; set; }
		private PersistentContainer container;
		
		
		public PersistentInterceptor(PersistentContainer container)
		{
			this.container	= container;
			this.Active		= false;
		}
		
		
		public void Intercept(IInvocation invocation)
		{
			if (this.Active)
			{
				PropertyInfo property = invocation.TargetType.GetProperty(ReflectHelper.GetPropertyName(invocation.Method));
				
				if (property != null)
				{
					if (ReflectHelper.IsGetter(invocation.Method))	invocation.ReturnValue = this.container.GetProperty(property);
					else 											this.container.SetProperty(property, invocation.Arguments[0]);
				}
				else switch (invocation.Method.Name)
				{
					case "GetSource": 	invocation.ReturnValue = this.container.Object;	break;
					case "GetBase":		invocation.ReturnValue = this.container;		break;
				}
			}

			//invocation.Proceed();
		}
	}
	
	
	internal class PersistentProxyGeneration : IProxyGenerationHook
	{
		public bool ShouldInterceptMethod(Type type, MethodInfo method)
		{
			return ReflectHelper.IsGetter(method) || ReflectHelper.IsSetter(method) || method.Name == "GetSource" || method.Name == "GetBase";	
		}
		
		
		public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
		{
		}
		
		
		public void MethodsInspected()
		{
				
		}
	}
}
		