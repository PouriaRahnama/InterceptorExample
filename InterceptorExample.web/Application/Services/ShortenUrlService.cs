using InterceptorExample.web.Domain;
using InterceptorExample.web.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterceptorExample.web.Application.Services
{
    public class ShortenUrlService
    {
        private readonly SqlServerApplicationDbContext _context;

        public ShortenUrlService(SqlServerApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateShortenLink(string destinationLink, CancellationToken cancellationToken)
        {
            string shortenCode = GenerateCode();
            Link link = new()
            {
                shortenUrl = shortenCode,
                destenationUrl = destinationLink
            };
            await _context.Links.AddAsync(link,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return $"https://localhost:7084/{shortenCode}";
        }

        public string GenerateCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 5);
        }

        public async Task<string> GetDestinationUrlAsync(string shortenCode, CancellationToken cancellationToken)
        {
            var destination = await _context.Links.FirstOrDefaultAsync(p => p.shortenUrl == shortenCode);

            //if (destination is null) throw new ArgumentNullException(nameof(destination));
            if (destination is null) return string.Empty;
            return destination.destenationUrl;
        }
    }
}
