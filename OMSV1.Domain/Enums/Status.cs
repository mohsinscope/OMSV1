namespace OMSV1.Domain.Enums;

public enum Status
{
    New = 0, // Initial state
    SentToProjectCoordinator = 1,
    ReturnedToProjectCoordinator = 2,
    SentToManager = 3,
    ReturnedToManager = 4,
    SentToDirector = 5,
    SentToAccountant = 6,
    ReturnedToSupervisor = 7,
    RecievedBySupervisor = 8,
    Completed = 9
}
