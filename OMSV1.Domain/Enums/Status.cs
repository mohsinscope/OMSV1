namespace OMSV1.Domain.Enums;

public enum Status
{
    New = 0, // Initial state
    SentToProjectCoordinator = 1,//المشرف دز لأية
    ReturnedToProjectCoordinator = 2,
    SentToManager = 3,//اية دزت لعبدالله
    ReturnedToManager = 4,
    SentToDirector = 5,//عبدالله دز لاسامة
    SentToAccountant = 6,
    ReturnedToSupervisor = 7,
    RecievedBySupervisor = 8,
    Completed = 9,
    //SentToAccountant
    SentFromDirector=10,//اسامة دز للمدقق
    //المدقق من يريد يرجع,يرجع لأية
    ReturnedToExpendeAuditer=11,//مدير الحسابات يرجع للمدقق
    SentToExpenseManager=12,//المدقق دز لمدير الحسابات
    ReturnedToExpenseManager=13,//ابو لبنى يرجع لأية
    SentToExpenseGeneralManager=14//مدير الحسابات دز لأبو لبنى
}
