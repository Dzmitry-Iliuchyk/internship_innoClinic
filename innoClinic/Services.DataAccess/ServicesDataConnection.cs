using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using Services.DataAccess;
using Services.Domain;
using System;

namespace Services.DataAccess {
    public class ServicesDataConnection: DataConnection {
        public ServicesDataConnection( DataOptions<ServicesDataConnection> options ) : base( options.Options ) {
        }
        public ITable<Service> Services => this.GetTable<Service>();
        public ITable<ServiceCategory> ServiceCategories => this.GetTable<ServiceCategory>();
        public ITable<Specialization> Specializations => this.GetTable<Specialization>();
    }
}
 public class ServicesMappingSchema: MappingSchema {
    public ServicesMappingSchema() {

        var builder =  new FluentMappingBuilder();
        builder.Entity<Service>()
            .HasTableName( "Services" )
            .Property( s => s.Id )
            .IsPrimaryKey()
            .IsIdentity()

            .Property( s => s.Name )
            .IsNullable( false )

            .Property( s => s.CategoryId )
            .IsNullable( false )

            .Property( s => s.SpecializationId )
            .IsNullable( false ) 
            
            .Property( s => s.Price )
            .IsNullable( false );
        
        builder.Entity<ServiceCategory>()
            .HasTableName( "ServiceCategories" )
            .Property( sc => sc.Id )
            .IsPrimaryKey()
            .IsIdentity()
            
            .Property( sc => sc.Name )
            .IsNullable( false );

        builder.Entity<Specialization>()
            .HasTableName( "Specialization" )
            .Property( sc => sc.Id )
            .IsPrimaryKey()
            .IsIdentity()
            
            .Property( sc => sc.Name )
            .IsNullable( false );
    } 
}