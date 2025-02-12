using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Domain.Appartments;
using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories
{
    internal sealed class AppartmentRepository : Repository<Appartment>, IAppartmentRepository
    {
        public AppartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
