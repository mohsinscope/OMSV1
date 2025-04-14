namespace OMSV1.Application.Dtos.Documents
{
    public class DocumentHistoryDto
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int ActionType { get; set; } // Using int since DocumentActions is an enum.
        public Guid ProfileId { get; set; }
        public DateTime ActionDate { get; set; }
        public string? Notes { get; set; }
        public DateTime Datecreated { get; set; }

    }
}
