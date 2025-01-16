namespace OMSV1.Domain.Enums;

public enum Status
{
    New = 0, // Initial state
    SentToProjectCoordinator = 1,
    ReturnedToProjectCoordinator = 2,
    SentToManager = 3,
    ReturnedToManager = 4,
    SentToDirector = 5,
    ReturnedToDirector = 6,

    SentToAccountant = 7,
    ReturnedToSupervisor = 8,
    RecievedBySupervisor = 9,
    Completed = 10
}
