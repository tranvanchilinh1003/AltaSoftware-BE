using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class RolePermissionRepo
    {
        private readonly isc_dbContext _context;
        public RolePermissionRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<RolePermission> GetRolePermissions()
        {
            return _context.RolePermissions.ToList();
        }

        public RolePermission GetRolePermissionById(long id)
        {

            return _context.RolePermissions.FirstOrDefault(s => s.Id == id);
        }

        public RolePermission CreateRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            _context.SaveChanges();
            return rolePermission;
        }

        public RolePermission? UpdateRolePermission(long id, RolePermissionRequest updated)
        {
            var existing = GetRolePermissionById(id);

            if (existing == null)
            {
                return null;
            }

            existing.PermissionId = updated.PermissionId;
            existing.RoleId = updated.RoleId;
            existing.Active = updated.Active;

            _context.RolePermissions.Update(existing);
            _context.SaveChanges();

            return existing;
        }

        public bool DeleteRolePermission(long id)
        {
            var rolePermission = GetRolePermissionById(id);
            if (rolePermission != null)
            {
                _context.RolePermissions.Remove(rolePermission);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
