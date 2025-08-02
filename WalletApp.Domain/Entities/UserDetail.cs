

namespace WalletApp.Domain.Entities
{
    public class UserDetail : BaseEntity
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; } 
        public AppUser User { get; set; }
        
    }
}
