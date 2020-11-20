using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DbTools.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            AddScopeFormAssembly(services, Assembly.GetExecutingAssembly().FullName);
        }

        private static void AddScopeFormAssembly(IServiceCollection services, string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            IEnumerable<Type> interfaces = GetInterfaces(assembly);
            IEnumerable<Type> types = GetClasses(assembly);

            foreach (var implType in types)
            {
                AddScopeByName(services, implType, interfaces);
            }
        }

        private static IEnumerable<Type> GetInterfaces(Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   where t.IsInterface && t.IsPublic
                   select t;
        }

        private static IEnumerable<Type> GetClasses(Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   where !t.IsInterface && t.IsClass && t.IsPublic
                   select t;
        }

        private static void AddScopeByName(IServiceCollection services, Type implType, IEnumerable<Type> interfaces)
        {
            Type serviceType = interfaces.SingleOrDefault(t => t.Name == "I" + implType.Name);

            if (serviceType != null)
            {
                services.AddScoped(serviceType, implType);
            }
        }
    }
}
