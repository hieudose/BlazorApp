using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Model.Entities
{
    public class UserModel
    {
        public UserModel()
        {
            UserRoles = new List<UserRoleModel>();
        }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRoleModel> UserRoles { get; set; }
    }
}
