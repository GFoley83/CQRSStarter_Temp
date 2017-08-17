using CQRSStarter.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSStarter.DAL
{
    /// <summary>
    /// Database Context based on EntityFramework (CodeFirst).
    /// </summary>
    public class CQRSStarterContext : DbContext, IRepository
    {
        /// <summary>
        /// The logger.
        /// </summary>
        //private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(CQRSStarterContext));

        /// <summary>
        /// Initializes a new instance of the <see cref="CQRSStarterContext"/> class.
        /// </summary>
        public CQRSStarterContext()
        {
            //System.Data.Entity.Database.SetInitializer<CQRSStarterContext>(null);
            Configuration.LazyLoadingEnabled = false;            
            Database.SetInitializer(new CQRSStarterContextInit());
        }

        /// <summary>
        /// Gets or sets the <see cref="Todo">Todos</see>.
        /// </summary>
        public IDbSet<Todo> Todos { get; set; }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        public override async Task<int> SaveChangesAsync()
        {
            // Set modification date if an entity has been modified
            foreach(var entity in ChangeTracker.Entries<EntityBase>().Where(e => e.State == EntityState.Modified))
            {
                entity.Entity.Modified = DateTime.UtcNow;
            }

            try
            {
                return await base.SaveChangesAsync();
            }
            catch(DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                        .Where(e => !e.IsValid)
                        .Select(e => $"{e.Entry.Entity.GetType().Name} - Errors: {string.Join(", ", e.ValidationErrors.Select(v => v.PropertyName + ": " + v.ErrorMessage))}");

                string errorText = string.Join("\r\n", errors);
                //Logger.Error(string.Format("Saving to database failed (Errors: {0})", errorText), ex);
                throw;
            }
            catch(Exception ex)
            {
                //Logger.Error("Saving to database failed", ex);
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
         
    }

    public class CQRSStarterContextInit : DropCreateDatabaseIfModelChanges<CQRSStarterContext>
    {
        protected override void Seed(CQRSStarterContext context)
        {
            var todos = new List<Todo>
            {
                new Todo() { Text = "Buy milk" },
                new Todo() { Text = "Walk the dog", Completed = true, CompletedDate = DateTime.UtcNow.AddDays(-2).Date }
            };

            foreach(Todo std in todos)
            {
                context.Todos.Add(std);
            }

            base.Seed(context);
        }
    }
}
