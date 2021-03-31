using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PecMemberSearch.Common;
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
        private readonly IWebHostEnvironment _env;
        private IHttpContextAccessor _accessor;

        public IndexModel(ISearchService searchService,
                          ICaptchaVerificationService verificationService,
                          IOptions<CaptchaSettings> captchaSettings,
                          IWebHostEnvironment env,
                          IHttpContextAccessor accessor)
        {
            _searchService = searchService;
            _verificationService = verificationService;
            _captchaSettings = captchaSettings.Value;
            _accessor = accessor;
            _env = env;
            Input = new InputModel();
        }

        [BindProperty]
        public string CaptchaClientKey { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }

        [BindProperty]
        public string ErrorMassage { get; set; }

        [BindProperty]
        public bool ShowResult { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public List<ApplicantsViewModel> ResultList { get; set; }

        
        public class InputModel
        {
            [Required(ErrorMessage = "Անունը պարտադիր է")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Ազգանունը պարտադիր է")]
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

            if (ModelState.IsValid)
            {
                try
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
                        ShowResult = true;

                        if (Input != null)
                        {
                            var ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                            var resultDir = Path.Combine(_env.WebRootPath, "logs");
                            if (!Directory.Exists(resultDir))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(resultDir);
                            }
                            var resultFileName = DateTime.Now.ToString("MM.yyyy") + ".csv";
                            string resultFilePath = Path.Combine(resultDir, resultFileName);

                            StringBuilder csvRegisterLog = new StringBuilder();
                            string row = "SearchDate=" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + ", IP=" + ip + ", FirstName=" + Input.FirstName + ", LastName=" + Input.LastName +
                                          ", Passport=" + Input.Passport;
                            csvRegisterLog.Append(row);
                            csvRegisterLog.Append(Environment.NewLine);
                            System.IO.File.AppendAllText(resultFilePath, csvRegisterLog.ToString());
                        }

                    }
                    else
                    {
                        ErrorMassage = "Ես ռոբոտ չեմ դաշտը պարտադիր է";
                        PrepareData();
                    }
                  
                }
                catch (Exception ex)
                {
                    ErrorMassage = ex.Message;
                    PrepareData();

                }
            }
            else
            {
                PrepareData();
            }

            PrepareData();
            return Page();
        }
    }
}
