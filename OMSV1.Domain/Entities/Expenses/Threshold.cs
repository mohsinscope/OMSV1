using OMSV1.Domain.SeedWork;

namespace OMSV1.Domain.Entities.Expenses
{
    public class Threshold : Entity
    {
        public string Name { get; private set; } // Low, Medium, High
        public decimal MinValue { get; private set; }
        public decimal MaxValue { get; private set; }

        public Threshold(string name, decimal minValue, decimal maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException("MinValue cannot be greater than MaxValue.");

            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public void Update(string name, decimal minValue, decimal maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException("MinValue cannot be greater than MaxValue.");

            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
