using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class RoleRepo
    {
        private readonly isc_dbContext _context;
        public RoleRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public Role? GetRoleById(long id)
        {

            return _context.Roles.FirstOrDefault(s => s.Id == id);
        }

        public Role CreateRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role;
        }

        public Role? UpdateRole(long id, RoleRequest updatedRole)
        {
            var existingRole = GetRoleById(id);

            if (existingRole == null)
            {
                return null;
            }

            existingRole.Name = updatedRole.Name;
            existingRole.Description = updatedRole.Description;
            existingRole.Active = updatedRole.Active;

            _context.Roles.Update(existingRole);
            _context.SaveChanges();

            return existingRole;
        }


        public bool DeleteRole(long id)
        {
            var role = GetRoleById(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
