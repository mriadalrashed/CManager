using System;

namespace CManager.Application.Helpers
{
    // Helper class for working with GUID values
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

        // Parses a string into a GUID if it is valid
        // Throws an exception if the format is invalid
        public static Guid ParseGuid(string guidString)
        {
            if (IsValidGuid(guidString))
            {
                return  Guid.Parse(guidString);
            }
            throw new FormatException("Invalid GUID format.");
        }
    }
}
