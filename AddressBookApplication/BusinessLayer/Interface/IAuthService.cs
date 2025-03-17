using ModelLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAuthService
    {
        Task<string> Register(UserDTO userDTO);
        Task<string> Login(UserDTO userDTO);
        Task<string> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
    }
}
