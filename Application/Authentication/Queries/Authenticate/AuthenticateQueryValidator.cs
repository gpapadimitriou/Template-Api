using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Queries.Authenticate
{
    public class AuthenticateQueryValidator : AbstractValidator<AuthenticateQuery>
    {
        private readonly string Username_IsRequired = "Το UserName είναι υποχρεωτικό";

        private readonly string Password_IsRequired = "Το Password είναι υποχρεωτικό";
        public AuthenticateQueryValidator()
        {

            RuleFor(v => v.Username)
               .NotEmpty().WithMessage(Username_IsRequired);

            RuleFor(v => v.Password)
               .NotEmpty().WithMessage(Password_IsRequired);
        }
    }
}
