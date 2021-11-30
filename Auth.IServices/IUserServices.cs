using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.IServices
{
    public interface IUserServices
    {
        User FirstOrDefault(Func<User, bool> condition);
        
    }
}
