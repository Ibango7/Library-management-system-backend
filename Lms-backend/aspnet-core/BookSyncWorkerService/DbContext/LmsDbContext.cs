using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookSyncWorkerService.Context
{
    internal class LmsDbContext:DbContext
    {
        public LmsDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
