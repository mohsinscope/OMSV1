// using System;

// namespace OMSV1.Domain.ValueObjects
// {
//     public struct UtcDateTime
//     {
//         public DateTime Value { get; }

//         private UtcDateTime(DateTime value)
//         {
//             if (value.Kind != DateTimeKind.Utc)
//             {
//                 throw new ArgumentException("DateTime must be UTC", nameof(value));
//             }
//             Value = value;
//         }

//         public static UtcDateTime From(DateTime dateTime)
//         {
//             return new UtcDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
//         }

//         public override string ToString()
//         {
//             return Value.ToString("o"); // ISO 8601 format
//         }
//     }
// }
