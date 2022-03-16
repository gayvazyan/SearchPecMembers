using PecMemberSearch.ModelDb;
using PecMemberSearch.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PecMemberSearch.Services
{
    public class SearchService: ISearchService
    {
        private readonly SearchContext _content;
        private readonly NewSearchContext _newContent;
        public SearchService(SearchContext content, NewSearchContext newContent)
        {
            _content = content;
            _newContent = newContent;
        }

      public  List<ApplicantsViewModel> GetResultWithOutPassport(string firstName, string lastName)
        {
            List<Applicant> searchApplicant = _content.Applicant.Where(p => (p.ApplicantFirstName.Contains(firstName) && p.ApplicantLastName.Contains(lastName))).ToList();

            List<Applicant> searchNewApplicant = _newContent.Applicant.Where(p => (p.ApplicantFirstName.Contains(firstName) && p.ApplicantLastName.Contains(lastName))).ToList();



            List<ApplicantsViewModel> searchApplicantsViewModel = new List<ApplicantsViewModel>();

            foreach (var item in searchApplicant)
            {
                var searchCommunity = _content.Community.FirstOrDefault(p => p.CommunityId == item.TrainingCenterCommunityId);
                var searchRegion = _content.Region.FirstOrDefault(p => p.RegionId == searchCommunity.RegionId);
                var searchData = _content.TrainingCourse.FirstOrDefault(p => p.TrainingCourseCode == item.TrainingCourseCode);
                DateTime? dataValide = item.CertificateIssue;
                string DataValide = null;
                if (dataValide != null)
                {
                    DateTime dateTime = (DateTime)dataValide;
                    DateTime dateTimeNew = dateTime.AddYears(3);
                    DataValide = dateTimeNew.ToString("dd.MM.yyyy");
                }


                string trainingRoom;
                string dataTraining;
                if (searchData != null)
                {
                    var serachTrainingRoom = _content.TrainingCenter.FirstOrDefault(p => p.TrainingCenterId == searchData.TrainingCenterId);
                    dataTraining = (searchData.DateTime).ToString("dd.MM.yyyy HH:mm");
                    trainingRoom = serachTrainingRoom.Address;
                }
                else
                {
                    trainingRoom = null;
                    dataTraining = null;
                }


                string CommunityName = searchCommunity.CommunityName;
                string RegionName = searchRegion.RegionName;
                string grupeTraining = item.TrainingCourseCode;


                ApplicantsViewModel applicantsViewModel = new ApplicantsViewModel
                {

                    //ApplicantId = item.ApplicantId,
                    FullName = item.ApplicantLastName + " " + item.ApplicantFirstName + " " + item.ApplicantMiddleName,
                    Address = item.Address,
                    TrainingCenter = grupeTraining + "," + CommunityName + "," + RegionName + "(" + trainingRoom + ")(" + dataTraining + ")",
                    // TrainingCenter = null,
                    Points = item.Points,
                    CertificateNumber = (item.CertificateNumber).ToString(),
                    Date = DataValide,
                    Comment = item.Comment
                };
                searchApplicantsViewModel.Add(applicantsViewModel);

            }



            foreach (var item in searchNewApplicant)
            {
                var searchCommunity = _newContent.Community.FirstOrDefault(p => p.CommunityId == item.TrainingCenterCommunityId);
                var searchRegion = _newContent.Region.FirstOrDefault(p => p.RegionId == searchCommunity.RegionId);
                var searchData = _newContent.TrainingCourse.FirstOrDefault(p => p.TrainingCourseCode == item.TrainingCourseCode);
                DateTime? dataValide = item.CertificateIssue;
                string DataValide = null;
                if (dataValide != null)
                {
                    DateTime dateTime = (DateTime)dataValide;
                    DateTime dateTimeNew = dateTime.AddYears(3);
                    DataValide = dateTimeNew.ToString("dd.MM.yyyy");
                }


                string trainingRoom;
                string dataTraining;
                if (searchData != null)
                {
                    var serachTrainingRoom = _newContent.TrainingCenter.FirstOrDefault(p => p.TrainingCenterId == searchData.TrainingCenterId);
                    dataTraining = (searchData.DateTime).ToString("dd.MM.yyyy HH:mm");
                    trainingRoom = serachTrainingRoom.Address;
                }
                else
                {
                    trainingRoom = null;
                    dataTraining = null;
                }


                string CommunityName = searchCommunity.CommunityName;
                string RegionName = searchRegion.RegionName;
                string grupeTraining = item.TrainingCourseCode;


                ApplicantsViewModel applicantsViewModel = new ApplicantsViewModel
                {

                    //ApplicantId = item.ApplicantId,
                    FullName = item.ApplicantLastName + " " + item.ApplicantFirstName + " " + item.ApplicantMiddleName,
                    Address = item.Address,
                    TrainingCenter = grupeTraining + "," + CommunityName + "," + RegionName + "(" + trainingRoom + ")(" + dataTraining + ")",
                    // TrainingCenter = null,
                    Points = item.Points,
                    CertificateNumber = (item.CertificateNumber).ToString(),
                    Date = DataValide,
                    Comment = item.Comment
                };
                searchApplicantsViewModel.Add(applicantsViewModel);

            }

            return searchApplicantsViewModel;
        }

      public List<ApplicantsViewModel> GetResultWithPassport(string firstName, string lastName, string passport)
        {
            List<Applicant> searchApplicant = _content.Applicant.Where(p => (p.ApplicantFirstName.Contains(firstName) && p.ApplicantLastName.Contains(lastName) && p.PassportNumber == passport)).ToList();
         
            List<Applicant> searchNewApplicant = _newContent.Applicant.Where(p => (p.ApplicantFirstName.Contains(firstName) && p.ApplicantLastName.Contains(lastName) && p.PassportNumber == passport)).ToList();


            List<ApplicantsViewModel> searchApplicantsViewModel = new List<ApplicantsViewModel>();

            foreach (var item in searchApplicant)
            {

                var searchCommunity = _content.Community.FirstOrDefault(p => p.CommunityId == item.TrainingCenterCommunityId);
                var searchRegion = _content.Region.FirstOrDefault(p => p.RegionId == searchCommunity.RegionId);
                var searchData = _content.TrainingCourse.FirstOrDefault(p => p.TrainingCourseCode == item.TrainingCourseCode);
                DateTime? dataValide = item.CertificateIssue;
                string DataValide = null;
                if (dataValide != null)
                {
                    DateTime dateTime = (DateTime)dataValide;
                    DateTime dateTimeNew = dateTime.AddYears(3);
                    DataValide = dateTimeNew.ToString("dd.MM.yyyy");
                }


                string trainingRoom;
                string dataTraining;
                if (searchData != null)
                {
                    var serachTrainingRoom = _content.TrainingCenter.FirstOrDefault(p => p.TrainingCenterId == searchData.TrainingCenterId);
                    dataTraining = (searchData.DateTime).ToString("dd.MM.yyyy HH:mm");
                    trainingRoom = serachTrainingRoom.Address;
                }
                else
                {
                    trainingRoom = null;
                    dataTraining = null;
                }

                string CommunityName = searchCommunity.CommunityName;
                string RegionName = searchRegion.RegionName;
                string grupeTraining = item.TrainingCourseCode;

                ApplicantsViewModel applicantsViewModel = new ApplicantsViewModel
                {

                    //ApplicantId = item.ApplicantId,
                    FullName = item.ApplicantLastName + " " + item.ApplicantFirstName + " " + item.ApplicantMiddleName,
                    Address = item.Address,
                    TrainingCenter = grupeTraining + "," + CommunityName + "," + RegionName + "(" + trainingRoom + ")(" + dataTraining + ")",
                    Points = item.Points,
                    CertificateNumber = (item.CertificateNumber).ToString(),
                    Date = DataValide,
                    Comment = item.Comment
                };
                searchApplicantsViewModel.Add(applicantsViewModel);

            }

           
            foreach (var item in searchNewApplicant)
            {

                var searchCommunity = _newContent.Community.FirstOrDefault(p => p.CommunityId == item.TrainingCenterCommunityId);
                var searchRegion = _newContent.Region.FirstOrDefault(p => p.RegionId == searchCommunity.RegionId);
                var searchData = _newContent.TrainingCourse.FirstOrDefault(p => p.TrainingCourseCode == item.TrainingCourseCode);
                DateTime? dataValide = item.CertificateIssue;
                string DataValide = null;
                if (dataValide != null)
                {
                    DateTime dateTime = (DateTime)dataValide;
                    DateTime dateTimeNew = dateTime.AddYears(3);
                    DataValide = dateTimeNew.ToString("dd.MM.yyyy");
                }


                string trainingRoom;
                string dataTraining;
                if (searchData != null)
                {
                    var serachTrainingRoom = _newContent.TrainingCenter.FirstOrDefault(p => p.TrainingCenterId == searchData.TrainingCenterId);
                    dataTraining = (searchData.DateTime).ToString("dd.MM.yyyy HH:mm");
                    trainingRoom = serachTrainingRoom.Address;
                }
                else
                {
                    trainingRoom = null;
                    dataTraining = null;
                }

                string CommunityName = searchCommunity.CommunityName;
                string RegionName = searchRegion.RegionName;
                string grupeTraining = item.TrainingCourseCode;

                ApplicantsViewModel applicantsViewModel = new ApplicantsViewModel
                {

                    //ApplicantId = item.ApplicantId,
                    FullName = item.ApplicantLastName + " " + item.ApplicantFirstName + " " + item.ApplicantMiddleName,
                    Address = item.Address,
                    TrainingCenter = grupeTraining + "," + CommunityName + "," + RegionName + "(" + trainingRoom + ")(" + dataTraining + ")",
                    Points = item.Points,
                    CertificateNumber = (item.CertificateNumber).ToString(),
                    Date = DataValide,
                    Comment = item.Comment
                };
                searchApplicantsViewModel.Add(applicantsViewModel);

            }

            return searchApplicantsViewModel;
        }
    }
}
