using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Entities
{
    public class Badge: Entity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
    }
}


