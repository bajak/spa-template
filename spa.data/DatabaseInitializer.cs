using System;
using System.Data.Entity;
using System.Web.Security;
using WebMatrix.WebData;

namespace spa.data
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        private readonly Action _preSeedAction;

        public DatabaseInitializer(Action preSeedAction)
        {
            _preSeedAction = preSeedAction;
        }

        protected override void Seed(DatabaseContext context)
        {
            _preSeedAction();

            const string username = "admin";
            const string role = "administrators";
            const string password = "admin";

            WebSecurity.CreateUserAndAccount(username, password, new { LastActivity = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc) });
            if (!Roles.RoleExists(role))
                Roles.CreateRole(role);
            if (!Roles.IsUserInRole(username, role))
                Roles.AddUserToRole(username, "administrators");

            context.SaveChanges();
        }
    }
}
