using KnowledgeSpace.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace KnowledgeSpace.BackendServer.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string AdminRoleName = "Admin";
        private readonly string UserRoleName = "Member";

        public DbInitializer(ApplicationDbContext context, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            #region =============== ROLE =============
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = AdminRoleName,
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper()
                });

                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = UserRoleName,
                    Name = UserRoleName,
                    NormalizedName = UserRoleName.ToUpper()
                });
            }

            #endregion =============== ROLE =============


            #region =============== USER =============
            if (!_userManager.Users.Any())
            {
                var result = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    FirstName = "Quan tri",
                    LastName = "1",
                    Email = "knowledgespace.tedu@gamil.com",
                    LockoutEnabled = false
                }, "Admin@123");
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, AdminRoleName);
                }
            }

            #endregion =============== USER =============


            #region =============== FUNCTION =============
            if (!_context.Functions.Any())
            {
                _context.Functions.AddRange(new List<Function>
                {
                    new Function { Id = "DASHBOARD", Name = "Satisfy", ParentId = null, SortOrder = 1, Url = "/dashboard" },

                    new Function { Id = "CONTENT", Name = "Intension", ParentId = null, Url = "/content" },
                    new Function { Id = "CONTENT_CATEGORY", Name = "Category", ParentId = "CONTENT", Url = "/content/category" },
                    new Function { Id = "CONTENT_KNOWLEDGEBASE", Name = "Article", ParentId = "CONTENT", SortOrder = 2, Url = "/content/kb" },
                    new Function { Id = "CONTENT_COMMENT", Name = "Page", ParentId = "CONTENT", SortOrder = 3, Url = "/content/comment" },
                    new Function { Id = "CONTENT_REPORT", Name = "Warning report", ParentId = "CONTENT", SortOrder = 3, Url = "/content/report" },

                    new Function { Id = "STATISTIC", Name = "Statistic", ParentId = null, Url = "/statistic" },

                    new Function { Id = "STATISTIC_MONTHLY_NEWMEMBER", Name = "Monthly register", ParentId = "STATISTIC", SortOrder = 1, Url = "/statistic/monthly-register" },
                    new Function { Id = "STATISTIC_MONTHLY_NEWKB", Name = "Monthly post", ParentId = "STATISTIC", SortOrder = 2, Url = "/statistic/monthly-newkb" },
                    new Function { Id = "STATISTIC_MONTHLY_COMMENT", Name = "Monthly comment", ParentId = "STATISTIC", SortOrder = 3, Url = "/statistic/monthly-comment" },

                    new Function { Id = "SYSTEM", Name = "Operation", ParentId = null, Url = "/system" },

                    new Function { Id = "SYSTEM_USER", Name = "User", ParentId = "SYSTEM", Url = "/system/user" },
                    new Function { Id = "SYSTEM_ROLE", Name = "Role group", ParentId = "SYSTEM", Url = "/system/role" },
                    new Function { Id = "SYSTEM_FUNCTION", Name = "Function", ParentId = "SYSTEM", Url = "/system/function" },
                    new Function { Id = "SYSTEM_PERMISSION", Name = "Permission", ParentId = "SYSTEM", Url = "/system/permission" },
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>()
                {
                    new Command(){Id = "VIEW", Name = "Show"},
                    new Command(){Id = "CREATE", Name = "Create"},
                    new Command(){Id = "UPDATE", Name = "Update"},
                    new Command(){Id = "DELETE", Name = "Delete"},
                    new Command(){Id = "APPROVE", Name = "Browse"}
                });
            }


            #endregion =============== FUNCTION =============


            #region =============== COMMAND IN FUNCTION =============
            var functions = _context.Functions;

            if (!_context.CommandInFunctions.Any())
            {
                foreach (var function in functions)
                {
                    var createAcction = new CommandInFunction()
                    {
                        CommandId = "CREATE",
                        FunctionId = function.Id,
                    };
                    _context.CommandInFunctions.Add(createAcction);

                    var updateAction = new CommandInFunction()
                    {
                        CommandId = "UPDATE",
                        FunctionId = function.Id,
                    };
                    _context.CommandInFunctions.Add(updateAction);

                    var deleteAction = new CommandInFunction()
                    {
                        CommandId = "DELETE",
                        FunctionId = function.Id,
                    };
                    _context.CommandInFunctions.Add(deleteAction);

                    var viewAction = new CommandInFunction()
                    {
                        CommandId = "VIEW",
                        FunctionId = function.Id,
                    };
                    _context.CommandInFunctions.Add(viewAction);

                }
            }

            #endregion =============== COMMAND IN FUNCTION =============


            #region =============== PERMISSION =============
            if (!_context.Permissions.Any())
            {
                var adminRole = await _roleManager.FindByNameAsync(AdminRoleName);
                foreach(var function in functions)
                {
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "CREATE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "UPDATE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "DELETE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "VIEW"));
                }
            }

            #endregion =============== PERMISSION =============

            await _context.SaveChangesAsync();
        }
    }
}
