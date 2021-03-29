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
        private readonly ILogger<IndexModel> _logger;
        private readonly ISearchService _searchService;
        private readonly CaptchaVerificationService _verificationService;
        private CaptchaSettings _captchaSettings;

        public IndexModel(ILogger<IndexModel> logger, 
                          ISearchService searchService,
                          CaptchaVerificationService verificationService,
                          IOptions<CaptchaSettings> captchaSettings)
        {
            _logger = logger;
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
            var firstName = Input.FirstName;
            var lastName = Input.LastName;
            var passport = Input.Passport;



            var requestIsValid = await _verificationService.IsCaptchaValid(CaptchaResponse);

            if (requestIsValid==true)
            {
                if (passport == null)
                {
                    ResultList = _searchService.GetResultWithOutPassport(firstName, lastName);
                }
                else
                {
                    ResultList = _searchService.GetResultWithPassport(firstName, lastName, passport);
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
