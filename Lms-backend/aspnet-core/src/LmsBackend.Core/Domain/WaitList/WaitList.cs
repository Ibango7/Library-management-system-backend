using Abp.Domain.Entities;
using LmsBackend.Authorization.Users;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Entities
{
    public class WaitList:Entity<Guid>
    {
        // Foreign keys
        public virtual long UserId { get; set; }
        public virtual User User { get; set; }

        public virtual Guid BookId { get; set; }
        public virtual Book Book { get; set; }

    }
}
