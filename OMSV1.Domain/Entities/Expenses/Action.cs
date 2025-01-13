using System;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class Action(string actionType, string notes, Guid profileId,Guid monthlyExpensesId) : Entity
{
    public string ActionType { get; private set; } = actionType;
    public string Notes { get; private set; } = notes;
    public Guid ProfileId { get; private set; } = profileId;
    public Guid MonthlyExpensesId { get; private set; } = monthlyExpensesId;
    public Profile? Profile { get; private set; }
    public MonthlyExpenses? MonthlyExpenses { get; private set; }


}
