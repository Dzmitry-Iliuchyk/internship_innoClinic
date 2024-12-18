using Microsoft.EntityFrameworkCore;
using Services.Domain;

namespace Services.DataAccess {
    public static class Seeder {
        public static void Seed( DbContext context ) {
            if (!context.Set<ServiceCategory>().Any()) {
                context.Set<ServiceCategory>().AddRange(
                    Enumerable.Range( 1, 10 ).Select( i => new ServiceCategory {
                        Name = $"Category {i}",
                        TimeSlotSize = TimeSpan.FromMinutes( 30 + i * 5 )
                    } )
                );
                context.SaveChanges();
            }

            if (!context.Set<Specialization>().Any()) {
                context.Set<Specialization>().AddRange(
                    Enumerable.Range( 1, 10 ).Select( i => new Specialization {
                        Name = $"Specialization {i}",
                        IsActive = i % 2 == 0
                    } )
                );
                context.SaveChanges();
            }

            if (!context.Set<Service>().Any()) {
                context.Set<Service>().AddRange(
                    Enumerable.Range( 1, 10 ).Select( i => new Service {
                        Id = Guid.NewGuid(),
                        Name = $"Service {i}",
                        Price = 100 + i * 10,
                        CategoryId = i,
                        SpecializationId = i,
                        IsActive = i % 2 == 0
                    } )
                );
                context.SaveChanges();
            }
        }

    }
}