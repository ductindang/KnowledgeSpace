using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.UnitTest.Extensions;
using KnowledgeSpace.ViewModels.Other;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{
    public class RolesControllerTest
    {
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private List<IdentityRole> _roleResources = new List<IdentityRole>()
                {
                    new IdentityRole("test1"),
                    new IdentityRole("test2"),
                    new IdentityRole("test3"),
                    new IdentityRole("test4"),
                };
        
        public RolesControllerTest() {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }
        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var roleController = new RolesController(_mockRoleManager.Object);

            Assert.NotNull(roleController);
        }

        [Fact]
        public async Task PostRole_ValidInput_Success()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.PostRole(new RoleVM()
            {
                Id = "test",
                Name = "test",
            });

            Assert.NotNull(result);
            Assert.IsType< CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task PostRole_ValidInput_Fail()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.PostRole(new RoleVM()
            {
                Id = "test",
                Name = "test",
            });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetRoles_HasData_Success()
        {
            // Sử dụng Extension AsAsyncQueryable: vì không thể sử dụng AsQueryable vì nó có async
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Returns(_roleResources.AsAsyncQueryable());
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.GetRoles();
            var okResult = result as OkObjectResult;
            var roleVM = okResult.Value as IEnumerable<RoleVM>;

            Assert.True(roleVM.Count() > 0);
        }

        [Fact]
        public async Task GetRoles_ThrowException_Fail()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Throws<Exception>();
            var roleController = new RolesController(_mockRoleManager.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await roleController.GetRoles());
        }

        [Fact]
        public async Task GetRolesPaging_NoFilter_Success()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Returns(_roleResources.AsAsyncQueryable());
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.GetRoles(null, 1, 2);
            var okResult = result as OkObjectResult;
            var roleVM = okResult.Value as Pagination<RoleVM>;

            Assert.Equal(4, roleVM.TotalRecords);
            Assert.Equal(2, roleVM.Items.Count());
        }

        [Fact]
        public async Task GetRolesPaging_NoFilter_Fail()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Throws<Exception>();
            var roleController = new RolesController(_mockRoleManager.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await roleController.GetRoles(null, 1, 1));
        }

        [Fact]
        public async Task GetRolesPaging_HasFilter_Success()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Returns(_roleResources.AsAsyncQueryable());
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.GetRoles("test3", 1, 2);
            var okResult = result as OkObjectResult;
            var roleVM = okResult.Value as Pagination<RoleVM>;

            Assert.Equal(1, roleVM.TotalRecords);
            Assert.Single(roleVM.Items);
        }

        [Fact]
        public async Task GetRolesPaging_HasFilter_Fail()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.Roles)
                .Throws<Exception>();
            var roleController = new RolesController(_mockRoleManager.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await roleController.GetRoles("1", 1, 1));
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test1",
                    Name = "test1"
                });
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.GetById("test1");
            var okResult = result as OkObjectResult;
            var roleVM = okResult.Value as RoleVM;

            Assert.NotNull(okResult);
            Assert.Equal("test1", roleVM.Name);
        }

        [Fact]
        public async Task GetById_ThrowException_Fail()
        {
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .Throws<Exception>();
            var roleController = new RolesController(_mockRoleManager.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await roleController.GetById("test1"));
        }

        [Fact]
        public async Task PutRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.PutRole("test", new RoleVM()
            {
                Id = "test",
                Name = "test",
            });

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutRole_ValidInput_Fail()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.PutRole("test", new RoleVM()
            {
                Id = "test",
                Name = "test",
            });

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.DeleteRole("test");

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteRole_ValidInput_Fail()
        {
            _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityRole()
                {
                    Id = "test",
                    Name = "test"
                });
            // Gia lap role manager va phuong thuc create
            _mockRoleManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError[] { }));
            var roleController = new RolesController(_mockRoleManager.Object);

            var result = await roleController.DeleteRole("test");

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }




}
 