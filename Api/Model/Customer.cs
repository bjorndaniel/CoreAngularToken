namespace AuthTokens.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }
        public string Loacation { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
    }
}