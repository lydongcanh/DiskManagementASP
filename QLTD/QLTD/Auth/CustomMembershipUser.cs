using Ehr.Models;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace Ehr.Auth
{
    public class CustomMembershipUser : MembershipUser
    {
        public int Id { get; set; }
        public ICollection<Role> Roles { get; set; }

        public CustomMembershipUser(User user) : base("CustomMembership", user.Username, user.Id, user.Username, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Id = user.Id;
            Roles = user.Roles;
        }
    }
}