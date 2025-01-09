namespace OMSV1.Domain.Enums;

public enum Status
{
    New = 0, // Initial state
    SentToProjectCoordinator = 1,
    SentToManager = 2,
    SentToDirector = 3,
    SentToAccountant = 4,
    ReturnedToSupervisor = 5,
    Completed = 6
}
