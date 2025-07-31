namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public Adress Adress { get; set; }

    }
}
