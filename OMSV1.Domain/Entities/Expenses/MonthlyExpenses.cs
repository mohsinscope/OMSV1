using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class MonthlyExpenses(Status status, decimal totalAmount, string notes, int officeId, int governorateId, int profileId) : Entity
{

    public decimal TotalAmount { get; private set; } = totalAmount;
    public Status Status { get; private set; } = status;
    public string Notes { get; private set; } = notes;

    public int OfficeId { get; private set; } = officeId;
    public int GovernorateId { get; private set; } = governorateId;
    public int ProfileId { get; private set; } = profileId;

    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }
    public Profile? Profile { get; private set; }

    private List<Action> _actions = new List<Action>();
    public IReadOnlyCollection<Action> actions => _actions.AsReadOnly();

    private List<DailyExpenses> _dailyExpenses = new List<DailyExpenses>();
    public IReadOnlyCollection<DailyExpenses> dailyExpenses => _dailyExpenses.AsReadOnly();

}
