using Application.Common.Extensions;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Specifications.User
{
    public class UserFilterSpecification : Specification<Domain.Entities.User>
    {
        public UserFilterSpecification(string email, string firstname, string lastname, string phone, string role, long dateCreatedFrom, long dateCreatedUntil, long lastLoginFrom, long lastLoginUntil, int page, int items, bool pagination = true)
        {
            if (!string.IsNullOrWhiteSpace(email))
                Query
                .Where(x => x.Email.Equals(email));

            if (!string.IsNullOrWhiteSpace(firstname))
                Query
                .Where(x => x.FirstName.Equals(firstname));

            if (!string.IsNullOrWhiteSpace(lastname))
                Query
                .Where(x => x.LastName.Equals(lastname));

            if (!string.IsNullOrWhiteSpace(phone))
                Query
                .Where(x => x.Phone.Equals(phone));

            if (!string.IsNullOrWhiteSpace(role))
                Query
                .Where(x => x.Role.Equals(role));

            if ((dateCreatedFrom != 0) && (dateCreatedUntil != 0))
            {
                Query
                    .Where(b => ((DateTimeOffset)b.DateCreated).ToUnixTimeMilliseconds() >= dateCreatedFrom
                    &&
                   ((DateTimeOffset)b.DateCreated).ToUnixTimeMilliseconds() <= dateCreatedUntil);
            }

            if ((lastLoginFrom != 0) && (lastLoginUntil != 0))
            {
                Query
                    .Where(b => ((DateTimeOffset)b.LastLogin).ToUnixTimeMilliseconds() >= lastLoginFrom
                    &&
                   ((DateTimeOffset)b.LastLogin).ToUnixTimeMilliseconds() <= lastLoginUntil);
            }


            if (pagination)
                Query.AddPagination(page, items);
        }
    }
}
