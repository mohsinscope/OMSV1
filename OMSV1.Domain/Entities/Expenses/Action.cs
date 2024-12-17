using System;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class Action(ActionType actionType, string notes, int profileId,int monthlyExpensesId) : Entity
{
    public ActionType ActionType { get; private set; } = actionType;
    public string Notes { get; private set; } = notes;
    public int ProfileId { get; private set; } = profileId;
    public int MonthlyExpensesId { get; private set; } = monthlyExpensesId;

    public Profile Profile { get; private set; }
    public MonthlyExpenses MonthlyExpenses { get; private set; }


}
