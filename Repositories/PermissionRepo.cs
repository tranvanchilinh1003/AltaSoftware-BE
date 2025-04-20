using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using System.Security;

namespace ISC_ELIB_SERVER.Repositories
{
    public class PermissionRepo
    {
        private readonly isc_dbContext _context;
        public PermissionRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<Permission> GetPermissions()
        {
            return _context.Permissions.ToList();
        }

        public Permission GetPermissionById(long id)
        {

            return _context.Permissions.FirstOrDefault(s => s.Id == id);
        }

        public Permission CreatePermission(Permission permission)
        {
            _context.Permissions.Add(permission);
            _context.SaveChanges();
            return permission;
        }

        public Permission? UpdatePermission(long id, PermissionRequest updated)
        {
            var existing = GetPermissionById(id);

            if (existing == null)
            {
                return null;
            }

            existing.Name = updated.Name;
            existing.Active = updated.Active;

            _context.Permissions.Update(existing);
            _context.SaveChanges();

            return existing;
        }

        public bool DeletePermission(long id)
        {
            var permission = GetPermissionById(id);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
