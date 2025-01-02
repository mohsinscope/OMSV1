using MediatR;
using OMSV1.Application.Dtos.Profiles;
using OMSV1.Application.Helpers;

namespace OMSV1.Application.Queries.Profiles
{
    public class SearchProfilesQuery : IRequest<PagedList<ProfileWithUserAndRolesDto>>
    {
        public string FullName { get; set; }          // Optional: Filter by full name
        public int? OfficeId { get; set; }           // Optional: Filter by office ID
        public int? GovernorateId { get; set; }      // Optional: Filter by governorate ID
        public List<string> Roles { get; set; } // New roles filter

        public PaginationParams PaginationParams { get; set; } // Pagination details
    }
}
