using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartifyBLL.ViewModels.User
{

    public class UserVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime JoinDate { get; set; }
        public string? AvatarUrl { get; set; }
        public int OrdersCount { get; set; }
        public bool EmailVerified { get; set; }

    }

}
