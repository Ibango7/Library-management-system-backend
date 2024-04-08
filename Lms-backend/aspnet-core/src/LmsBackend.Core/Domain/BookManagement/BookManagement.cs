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
    public class BookManagement: Entity<Guid>
    {
        public virtual bool Returned { get; set; }
        public virtual bool Overdue { get; set; }
        public virtual DateTime DateBorrowed { get; set; }

        public virtual Guid BookId { get; set; }
        public virtual Book Book { get; set; }

 
        public virtual long UserId { get; set; }     
        public virtual User User { get; set; }
    }

}
