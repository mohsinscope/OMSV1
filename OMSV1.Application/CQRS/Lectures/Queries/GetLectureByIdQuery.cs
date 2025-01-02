using MediatR;
using OMSV1.Application.Dtos.Lectures;  // Ensure you're using the correct DTO

namespace OMSV1.Application.Queries.Lectures
{
    public class GetLectureByIdQuery : IRequest<LectureDto?>  // Query now returns LectureDto
    {
        public Guid Id { get; }

        public GetLectureByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
