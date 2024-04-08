using Abp.Authorization;
using LmsBackend.Authorization.Roles;
using LmsBackend.Authorization.Users;

namespace LmsBackend.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
