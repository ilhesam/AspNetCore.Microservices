namespace Hotel.Services.Identity.Core.Models
{
    public class IdentityError
    {
        public IdentityError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }
    }
}