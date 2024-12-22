namespace OMSV1.Domain.Enums;
[Flags]

    public enum WorkingHours
    {
    None = 0,
    Morning = 1,
    Evening = 2,
    Both = Morning | Evening // Represents both Morning and Evening
    }