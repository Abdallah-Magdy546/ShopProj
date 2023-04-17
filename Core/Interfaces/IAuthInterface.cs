using Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthInterface
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
    }
}
