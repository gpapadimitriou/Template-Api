using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Specifications.User
{
    public class UserSpecification : Specification<Domain.Entities.User>
    {
        public UserSpecification(string applicationUserId)
        {
            Query
                .Where(p => p.ApplicationUserId.Equals(applicationUserId));
        }
    }
}
