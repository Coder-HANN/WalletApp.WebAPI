

namespace WalletApp.Domain.Base
{
    public abstract class BaseEntity    
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedUser { get; set; }
        public Guid Created { get; set; }
    }
}
