using Ehr.Data;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Ehr.Auth
{
    public class CustomRole 
    {
        private readonly QLTDDBContext context;

        public CustomRole(QLTDDBContext context)
        {
            this.context = context;
        }

        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };
            var selecteduser = (from u in context.Users.Include("Roles")
                                where string.Compare(u.Username, username, StringComparison.OrdinalIgnoreCase) == 0
                                select u).FirstOrDefault();
            if (selecteduser != null)
            {
                userRoles = new[] { selecteduser.Roles.Select(r => r.RoleName).ToString() };
            }

            return userRoles.ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}