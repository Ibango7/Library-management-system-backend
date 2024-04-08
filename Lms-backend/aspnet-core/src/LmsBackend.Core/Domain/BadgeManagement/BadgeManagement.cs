using Abp.Domain.Entities;
using LmsBackend.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Entities
{
    public class BadgeManagement: Entity<Guid>
    {
        public virtual long Rating { get; set; }

        // foreign key
        public virtual long UserId { get; set; }
        public virtual User User { get; set; }


        public virtual Guid BadgeId { get; set; }
        public virtual Badge Badge { get; set; }

    }
}
