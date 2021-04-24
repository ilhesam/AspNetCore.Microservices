using System;

namespace Hotel.Common.Models
{
    [Serializable]
    public class Error
    {
        // For deserialize
        private Error()
        {
        }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; private set; } // private set for deserialize
        public string Description { get; private set; } // private set for deserialize

        public static Error Default() => new("DEFAULT", "There is a problem");
    }
}