#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: AdventureWorksLt2012.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using DataLayer.Helpers;
using GenericServices;

namespace DataLayer.GeneratedEf
{
    public partial class AdventureWorksLt2012 : IGenericServicesDbContext
    {

        /// <summary>
        /// This has been overridden to handle:
        /// a) Updating of modified items (see p194 in DbContext book)
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {

            HandleChangeTracking();

            return base.SaveChanges();

        }

        /// <summary>
        /// This handles going through all the entities that have changed and seeing if we need to do anything.
        /// </summary>
        private void HandleChangeTracking()
        {
            //Debug.WriteLine("----------------------------------------------");
            //foreach (var entity in ChangeTracker.Entries()
            //.Where(
            //    e =>
            //    e.State == EntityState.Added || e.State == EntityState.Modified))
            //{
            //    Debug.WriteLine("Entry {0}, state {1}", entity.Entity, entity.State);
            //}       

            foreach (var entity in ChangeTracker.Entries()
                                                .Where(
                                                    e =>
                                                    e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                UpdateTrackedEntity(entity);
            }
        }

        /// <summary>
        /// Looks at everything that has changed and applies any further action if required.
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <returns></returns>
        private static void UpdateTrackedEntity(DbEntityEntry entityEntry)
        {
            var trackUpdateClass = entityEntry.Entity as IModifiedEntity;
            if (trackUpdateClass == null) return;
            trackUpdateClass.ModifiedDate = DateTime.UtcNow;
            if (entityEntry.State == EntityState.Added)
                trackUpdateClass.rowguid = Guid.NewGuid();
        }
    }
}
