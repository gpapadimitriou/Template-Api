using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public virtual bool? IsActive { get; set; } = true;
        public virtual DateTime? DateModified { get; set; }
    }
}
