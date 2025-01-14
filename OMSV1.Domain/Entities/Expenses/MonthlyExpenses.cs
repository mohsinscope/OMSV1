using System;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses;

public class MonthlyExpenses(Status status, decimal totalAmount, string notes, Guid officeId, Guid governorateId, Guid profileId) : Entity
{

    public decimal TotalAmount { get; private set; } = totalAmount;
    public Status Status { get; private set; } = status;
    public string? Notes { get; private set; } = notes;

    public Guid OfficeId { get; private set; } = officeId;
    public Guid GovernorateId { get; private set; } = governorateId;
    public Guid ProfileId { get; private set; } = profileId;

    public Governorate? Governorate { get; private set; }
    public Office? Office { get; private set; }
    public Profile? Profile { get; private set; }
    public Guid? ThresholdId { get; private set; }
    public Threshold? Threshold { get; private set; }

    private List<Action> _actions = new List<Action>();
    public IReadOnlyCollection<Action> actions => _actions.AsReadOnly();

    private List<DailyExpenses> _dailyExpenses = new List<DailyExpenses>();
    public IReadOnlyCollection<DailyExpenses> dailyExpenses => _dailyExpenses.AsReadOnly();
    
    //  public MonthlyExpenses(Status status, decimal totalAmount, string notes, Guid officeId, Guid governorateId, Guid profileId)
    // {
    //     if (totalAmount < 0) throw new ArgumentException("TotalAmount cannot be negative.");
    //     Status = status;
    //     TotalAmount = totalAmount;
    //     Notes = notes;
    //     OfficeId = officeId;
    //     GovernorateId = governorateId;
    //     ProfileId = profileId;
    // }

    // Method to add a daily expense
    public void AddDailyExpense(DailyExpenses dailyExpense, IEnumerable<Threshold> thresholds)
    {
        if (Status == Status.Completed)
            throw new InvalidOperationException("Cannot add daily expenses to a completed MonthlyExpenses.");

        if (dailyExpense == null)
            throw new ArgumentNullException(nameof(dailyExpense));

        _dailyExpenses.Add(dailyExpense);
        TotalAmount += dailyExpense.Amount;

        // Recalculate the threshold dynamically
        RecalculateThreshold(thresholds);
    }

    // Method to update status
    public void UpdateStatus(Status newStatus)
    {
        if (Status == Status.Completed)
            throw new InvalidOperationException("Cannot change the status of a completed MonthlyExpenses.");

        Status = newStatus;
    }
    //Add notes
        public void AddNotes(string notes)
    {
        Notes = notes;
    }
        // Add a method to adjust the TotalAmount
    public void AdjustTotalAmount(decimal adjustmentAmount)
    {
        if (Status == Status.Completed)
            throw new InvalidOperationException("Cannot adjust the total amount of a completed MonthlyExpenses.");

        if (TotalAmount + adjustmentAmount < 0)
            throw new InvalidOperationException("TotalAmount cannot be negative.");

        TotalAmount += adjustmentAmount;
    }
    //Threshold Assignemnt
         public void AssignThreshold(Threshold threshold)
        {
            if (TotalAmount < threshold.MinValue || TotalAmount > threshold.MaxValue)
                throw new InvalidOperationException("The total amount does not match the provided threshold.");

            ThresholdId = threshold.Id;
            Threshold = threshold;
        }
        //Recalculate threshold
    public void RecalculateThreshold(IEnumerable<Threshold> thresholds)
    {
        var matchingThreshold = thresholds.FirstOrDefault(t =>
            TotalAmount >= t.MinValue && TotalAmount <= t.MaxValue);

        if (matchingThreshold == null)
            throw new InvalidOperationException("No matching threshold found for the updated TotalAmount.");

        AssignThreshold(matchingThreshold);
    }


}
