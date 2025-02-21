using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;

}
