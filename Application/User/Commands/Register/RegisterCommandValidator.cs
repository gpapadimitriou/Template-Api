using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(v => v.Email)
               .NotEmpty().WithMessage("Το email είναι υποχρεωτικό");
            RuleFor(v => v.FirstName)
               .NotEmpty().WithMessage("Το FirstName είναι υποχρεωτικό");
            RuleFor(v => v.LastName)
               .NotEmpty().WithMessage("Το LastName είναι υποχρεωτικό");
            RuleFor(v => v.Phone)
               .NotEmpty().WithMessage("Το Τηλέφωνο είναι υποχρεωτικό");
            RuleFor(v => v.Password)
               .NotEmpty().WithMessage("Το Password είναι υποχρεωτικό");
            RuleFor(v => v.RePassword)
               .NotEmpty().WithMessage("Το RePassword είναι υποχρεωτικό").DependentRules(() =>
               {
                   RuleFor(v => new { v.Password, v.RePassword })
                   .Must(x => x.RePassword.Equals(x.Password))
                   .WithMessage("Το Password και το RePassword δεν ταιριάζουν");
               });
            RuleFor(v => v.Role)
               .NotEmpty().WithMessage("To Role είναι υποχρεωτικό");
        }
    }
}
