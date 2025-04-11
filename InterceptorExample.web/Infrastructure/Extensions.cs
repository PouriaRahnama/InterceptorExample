using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.Loader;
using InterceptorExample.web.Domain;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InterceptorExample.web.Infrastructure
{
    public static class Extensions
    {
        // Extension Method
        public static void OnCreated(this ModelBuilder modelBuilder)
        {
            var listEntities = typeof(ICreatedEntity).GetAllClassName();
            var listMapEntities =
                modelBuilder.Model.GetEntityTypes().Where(p => listEntities.Contains(p.ClrType.FullName));
            foreach (var map in listMapEntities)
            {
                var props = map.FindProperty("CreatedAt");
                if (props is null)
                {
                    map.AddProperty("CreatedAt", typeof(DateTime)).ValueGenerated = ValueGenerated.OnAdd;
                    map.AddProperty("CreatedByIP", typeof(string));
                    map.AddProperty("CreatedByBrowser", typeof(string));
                }
            }
        }

        public static List<string> GetAllClassName(this Type type)
        {
            var _lista = new List<Assembly>();
            foreach (string dllPath in Directory.GetFiles(AppContext.BaseDirectory, "InterceptorExample.*.dll"))
            {
                var shadowCopiedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                _lista.Add(shadowCopiedAssembly);
            }

            return _lista.SelectMany(x => x.GetTypes()).Where(x => type.IsAssignableFrom(x) & !x.IsInterface & !x.IsAbstract).Select(x => x.FullName).ToList();
        }
    }
}
