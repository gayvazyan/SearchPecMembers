using PecMemberSearch.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PecMemberSearch.Services
{
    public interface ISearchService
    {
        List<ApplicantsViewModel> GetResultWithOutPassport(string firstName, string lastName);
        List<ApplicantsViewModel> GetResultWithPassport(string firstName, string lastName, string passport);
    }
}
