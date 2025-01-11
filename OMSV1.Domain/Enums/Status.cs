namespace OMSV1.Domain.Enums;

public enum Status
{
    New = 0, // Initial state
    SentToProjectCoordinator = 1,
    ReturnedToProjectCoordinator = 2,
    SentToManager = 3,
    ReturnedToManager = 3,
    SentToDirector = 4,
    SentToAccountant = 5,
    ReturnedToSupervisor = 6,
    RecievedBySupervisor = 7,
    Completed = 8
}
