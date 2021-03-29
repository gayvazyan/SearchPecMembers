using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PecMemberSearch.Services;
using PecMemberSearch.Services.CaptchaVerification;
using PecMemberSearch.ViewModels;
using static PecMemberSearch.Services.CaptchaVerification.CaptchaVerificationService;

namespace PecMemberSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISearchService _searchService;
        private readonly ICaptchaVerificationService _verificationService;
        private CaptchaSettings _captchaSettings;

        public IndexModel(ISearchService searchService,
                          ICaptchaVerificationService verificationService,
                          IOptions<CaptchaSettings> captchaSettings)
        {
            _searchService = searchService;
            _verificationService = verificationService;
            _captchaSettings = captchaSettings.Value;
            Input = new InputModel();
        }

        public string CaptchaClientKey { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }

        [BindProperty]

        public string ErrorMassage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public List<ApplicantsViewModel> ResultList { get; set; }

        
        public class InputModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Passport { get; set; }
          
        }

        public void PrepareData()
        {
            CaptchaClientKey = _captchaSettings.SiteKey;
           
        }

        public void OnGet()
        {
            ErrorMassage = string.Empty;
            PrepareData();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var requestIsValid = await _verificationService.IsCaptchaValid(CaptchaResponse);

            if (requestIsValid==true)
            {
                if (Input.Passport == null)
                {
                    ResultList = _searchService.GetResultWithOutPassport(Input.FirstName, Input.LastName);
                }
                else
                {
                    ResultList = _searchService.GetResultWithPassport(Input.FirstName, Input.LastName, Input.Passport);
                }
            }

            else
            {
                ErrorMassage = "Նշեք Ես Ռոբոտ չեմ դաշտը։";
                PrepareData();
            }

            return Page();
        }
    }
}
