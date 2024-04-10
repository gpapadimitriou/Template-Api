using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extensions
{
    public static class SpecificationExtensions
    {
        public static ISpecificationBuilder<T> AddPagination<T>(this ISpecificationBuilder<T> @this, int page, int items)
        {
            if (page > 0 && items > 0)
            {
                return @this
                    .Skip((page - 1) * items)
                    .Take(items);
            }
            return @this.Take(0);
        }
    }
}
