using System.Collections.Generic;

namespace Hotel.Services.Identity.Core.Models
{
    public class IdentityResult
    {
        private IdentityResult(bool succeeded, List<IdentityError> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; }
        public List<IdentityError> Errors { get; set; }

        public static IdentityResult Success() => new IdentityResult(true, null);

        public static IdentityResult Fail(List<IdentityError> errors) => new IdentityResult(false, errors);
    }
}