using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace QDT.Core.MSData
{
    public class DB : DbContext
    {
        public const string TablePrefix = "qdt_";

        public DB(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //Configuration.LazyLoadingEnabled = true;
            //Configuration.ProxyCreationEnabled = false;
        }

        public static string TPref(string tableName)
        {
            return TablePrefix +  tableName;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            RegistryConfigMapping(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void RegistryConfigMapping(DbModelBuilder modelBuilder)
        {
            Func<Type, bool> filter = type => type.BaseType != null && (type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType &&
                                                                        type.BaseType.IsGenericType &&
                                                                        (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) || type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)) &&
                                                                        (type.GetConstructor(Type.EmptyTypes) != null));

            IEnumerable<Type> configurationTypes = typeof(DB).Assembly.GetTypes().Where(filter);

            foreach (var config in configurationTypes.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)config);
            }
        }
    }
}
