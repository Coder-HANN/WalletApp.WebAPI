using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Domain.Base
{
    public class UserDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
        public User User { get; set; }
    }
}
