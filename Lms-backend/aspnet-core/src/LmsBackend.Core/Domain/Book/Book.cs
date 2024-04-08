using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Entities
{
    public class Book : AuditedEntity<Guid>
    {
        public virtual string ISBN { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string Content { get; set; }
        public virtual string Author { get; set; }
        public virtual string Genre { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual int AccessFrequency { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int Shelf { get; set; }
    }
}
