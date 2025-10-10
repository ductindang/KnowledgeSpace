using KnowledgeSpace.ViewModels.Other;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostRole(RoleVM roleVM)
        {
            var role = new IdentityRole()
            {
                Id = roleVM.Id,
                Name = roleVM.Name,
                NormalizedName = roleVM.Name.ToUpper()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new {Id = role.Id}, roleVM);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = _roleManager.Roles;
            var roleVMs = await roles.Select(r => new RoleVM()
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToListAsync();

            return Ok(roleVMs);
        }

        [HttpGet("/filter")]
        public async Task<IActionResult> GetRoles(string filter, int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Id.Contains(filter) || x.Name.Contains(filter));
            }

            var totalRecords = await query.CountAsync();

            var item = await query.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .Select(r => new RoleVM()
                                {
                                    Id = r.Id,
                                    Name = r.Name
                                }).ToListAsync();
            var pagination = new Pagination<RoleVM>
            {
                Items = item,
                TotalRecords = totalRecords
            };

            return Ok(pagination);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            var roleVM = new RoleVM()
            {
                Id = role.Id,
                Name = role.Name
            };
            return Ok(roleVM);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, [FromBody]RoleVM roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            
            role.Name = roleVM.Name;
            role.NormalizedName = roleVM.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) {
                //return Ok(roleVM);
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                var roleVM = new RoleVM()
                {
                    Id = role.Id,
                    Name = role.Name
                };

                return Ok(roleVM);
            }

            return BadRequest(result.Errors);
        }
    }
}
