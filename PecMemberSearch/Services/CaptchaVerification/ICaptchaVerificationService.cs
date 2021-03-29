using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PecMemberSearch.Services.CaptchaVerification
{
    public interface ICaptchaVerificationService
    {
        Task<bool> IsCaptchaValid(string token);
    }
}
