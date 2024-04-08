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
    public class Notification:Entity<Guid>
    {
        public virtual DateTime timestamp { get; set; }
        public virtual string Type {  get; set; }
        public virtual string Body { get; set; }

        // Foreign Keys
    
        public long SenderId { get; set; }
        public virtual User Sender{ get; set; } 

        public long ReceiverId { get; set; }       
        public virtual User Receiver { get; set; }

    }
}
