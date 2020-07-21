using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PecMemberSearch.Services;
using PecMemberSearch.ViewModels;

namespace PecMemberSearch.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISearchService _searchService;

        public IndexModel(ILogger<IndexModel> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
            Input = new InputModel();
        }

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

        public void OnGet()
        {
           
        }
        public void OnPost()
        {
            var firstName = Input.FirstName;
            var lastName = Input.LastName;
            var passport = Input.Passport;

            if (passport == null)
            {
                ResultList = _searchService.GetResultWithOutPassport(firstName, lastName);
            }
            else
            {
                ResultList = _searchService.GetResultWithPassport(firstName, lastName, passport);
            }

        }
    }
}
