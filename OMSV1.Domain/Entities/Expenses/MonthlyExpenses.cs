using System;
using System.Collections.Generic;
using System.Linq;
using OMSV1.Domain.Entities.Governorates;
using OMSV1.Domain.Entities.Offices;
using OMSV1.Domain.Entities.Profiles;
using OMSV1.Domain.Enums;
using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses
{
    public class MonthlyExpenses : Entity
    {
        public decimal TotalAmount { get; private set; }
        public Status Status { get; private set; }

        public Guid OfficeId { get; private set; }
        public Guid GovernorateId { get; private set; }
        public Guid ProfileId { get; private set; }

        public DateTime DateCreated { get; private set; }  // ✅ Added

        public Governorate Governorate { get; private set; } = null!;
        public Office Office { get; private set; } = null!;
        public Profile Profile { get; private set; } = null!;
        public Guid? ThresholdId { get; private set; }
        public Threshold Threshold { get; private set; } = null!;

        private List<Action> _actions = new();
        public IReadOnlyCollection<Action> actions => _actions.AsReadOnly();

        private List<DailyExpenses> _dailyExpenses = new();
        public IReadOnlyCollection<DailyExpenses> dailyExpenses => _dailyExpenses.AsReadOnly();

        // ✅ Constructor updated to include DateCreated
        public MonthlyExpenses(Status status, decimal totalAmount, Guid officeId, Guid governorateId, Guid profileId, DateTime dateCreated)
        {
            Status = status;
            TotalAmount = totalAmount;
            OfficeId = officeId;
            GovernorateId = governorateId;
            ProfileId = profileId;
            DateCreated = dateCreated;
        }

        // ✅ Method to add a daily expense
        public void AddDailyExpense(DailyExpenses dailyExpense, IEnumerable<Threshold> thresholds)
        {
            if (Status == Status.Completed)
                throw new InvalidOperationException("Cannot add daily expenses to a completed MonthlyExpenses.");

            if (dailyExpense == null)
                throw new ArgumentNullException(nameof(dailyExpense));

            _dailyExpenses.Add(dailyExpense);
            TotalAmount += dailyExpense.Amount;

            RecalculateThreshold(thresholds);
        }

        // ✅ Update status method
        public void UpdateStatus(Status newStatus)
        {
            if (Status == Status.Completed)
                throw new InvalidOperationException("Cannot change the status of a completed MonthlyExpenses.");

            Status = newStatus;
        }

        // ✅ Adjust total amount
        public void AdjustTotalAmount(decimal adjustmentAmount)
        {
            if (Status == Status.Completed)
                throw new InvalidOperationException("Cannot adjust the total amount of a completed MonthlyExpenses.");

            if (TotalAmount + adjustmentAmount < 0)
                throw new InvalidOperationException("TotalAmount cannot be negative.");

            TotalAmount += adjustmentAmount;
        }

        // ✅ Threshold assignment
        public void AssignThreshold(Threshold threshold)
        {
            if (TotalAmount < threshold.MinValue || TotalAmount > threshold.MaxValue)
                throw new InvalidOperationException("The total amount does not match the provided threshold.");

            ThresholdId = threshold.Id;
            Threshold = threshold;
        }

        // ✅ Recalculate threshold
        public void RecalculateThreshold(IEnumerable<Threshold> thresholds)
        {
            var matchingThreshold = thresholds.FirstOrDefault(t =>
                TotalAmount >= t.MinValue && TotalAmount <= t.MaxValue);

            if (matchingThreshold == null)
                throw new InvalidOperationException("No matching threshold found for the updated TotalAmount.");

            AssignThreshold(matchingThreshold);
        }
    }
}
