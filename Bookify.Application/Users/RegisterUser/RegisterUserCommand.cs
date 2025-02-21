using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.RegisterUser
{
    public sealed record RegisterUserCommand (
            string Email,
            string FirstName,
            string LastName,
            string Password
        ) : ICommand<Guid>;
}
