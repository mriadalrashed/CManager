// NOTE (AI Assistance Disclosure):
// The documentation comments in this file were created with assistance from an AI tool.
// Reference:
// Microsoft Docs - System.Guid
// https://learn.microsoft.com/dotnet/api/system.guid

using System;

namespace CManager.Application.Helpers
{
    /// <summary>
    /// Helper class for working with GUID values.
    /// </summary>
    /// <remarks>
    /// The idea of extracting GUID-related logic into a helper class
    /// is based on SOLID principles, specifically the Single Responsibility Principle (SRP).
    /// This avoids duplicating GUID creation logic across the application
    /// and improves maintainability and testability.
    /// </remarks>
    public static class GuidHelper
    {
        // Generates and returns a new unique GUID
        public static Guid GenerateGuid()

        {  
         return Guid.NewGuid();
        }

        // Checks whether a string is a valid GUID format
        public static bool IsValidGuid(string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }

        /// <summary>
        /// Parses a string into a GUID if the format is valid.
        /// Thrown when the provided string is not a valid GUID format.
        /// </summary>
        public static Guid ParseGuid(string guidString)
        {
            // Validate the GUID string before parsing to ensure safe conversion
            if (IsValidGuid(guidString))
            {
                return  Guid.Parse(guidString);
            }
            throw new FormatException("Invalid GUID format.");
        }
    }
}
