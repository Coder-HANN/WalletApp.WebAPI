using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.User.Dtos.UserProfile
{
    public class UserProfileRequestDTO
    {
        public int AppUserId { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }

    }
}
