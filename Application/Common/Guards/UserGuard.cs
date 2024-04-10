using Application.Common.Exceptions;
using Ardalis.GuardClauses;
using Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Common.Guards
{
    public static class UserGuard
    {
        public static void InvalidUsername(this IGuardClause guardClause, ApplicationUser user)
        {
            if (user == null)
                throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure("Username","Το UserName δεν είναι σωστό")
            });
        }

        public static void InvalidCredentials(this IGuardClause guardClause, bool result)
        {
            if (result == false)
                throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure("Password","Το PassWord δεν είναι σωστό")
            });
        }

        public static void EmailIsTaken(this IGuardClause guardClause, ApplicationUser user)
        {
            if (user != null)
                throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure("Email","Το Email χρησιμοποιείται ήδη")
            });
        }

        public static void InvalidRole(this IGuardClause guardClause, List<string> roles, string role)
        {
            bool roleExists = roles.Any(s => role.Contains(s));
            if (roleExists != true)
                throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure("Role","Ο Ρόλος δεν είναι σωστός")
            });
        }
    }
}
