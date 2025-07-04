using InterceptorExample.web.Domain;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;

namespace InterceptorExample.web.Infrastructure.Common
{
    public sealed class CustomInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomInterceptor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateDbContext(eventData.Context, _contextAccessor);
            SoftDelete(eventData, result);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        private void UpdateDbContext(DbContext context, IHttpContextAccessor httpContextAccessor)
        {
            var CreatedList = context.ChangeTracker.Entries<ICreatedEntity>().Where(p => p.State == EntityState.Added).ToList();
            if (CreatedList.Count > 0)
            {
                foreach (var entry in CreatedList)
                {
                    entry.Property<DateTime>("CreatedAt").CurrentValue = DateTime.Now;
                    entry.Property<string>("CreatedByIP").CurrentValue =
                        httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() ?? "localhost";
                    entry.Property<string>("CreatedByBrowser").CurrentValue = httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString() ?? "localhost";
                }
            }
        }

        private void SoftDelete(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context == null)
                return;

            var _list = eventData.Context.ChangeTracker.Entries<ISoftDelete>()
                .Where(p => p.State == EntityState.Deleted);
            foreach (var item in _list)
            {
                item.State = EntityState.Modified;
                item.Entity.DeletedAt = DateTime.Now;
                item.Entity.IsDeleted = true;
            }
        }

    }


}
