using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ISC_ELIB_SERVER.Models;
using System;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ISystemSettingsRepo
    {
        ICollection<SystemSetting> GetAll();
        SystemSetting GetById(int id);
        SystemSetting Create(SystemSetting systemSetting);
        SystemSetting? Update(SystemSetting systemSetting);
        bool Delete(int id);
    }

    public class SystemSettingsRepo : ISystemSettingsRepo
    {
        private readonly isc_dbContext _context;

        public SystemSettingsRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<SystemSetting> GetAll()
        {
            return _context.SystemSettings
                .Include(s => s.User)
                .Include(s => s.Theme)
                .ToList();
        }

        public SystemSetting GetById(int id)
        {
            return _context.SystemSettings
                .Include(s => s.User)
                .Include(s => s.Theme)
                .FirstOrDefault(s => s.Id == id);
        }

        public SystemSetting Create(SystemSetting systemSetting)
        {
            _context.SystemSettings.Add(systemSetting);
            _context.SaveChanges();
            return systemSetting;
        }

        public SystemSetting? Update(SystemSetting systemSetting)
        {
            var existingSetting = _context.SystemSettings.Find(systemSetting.Id);

            if (existingSetting == null)
            {
                return null;
            }

            existingSetting.Captcha = systemSetting.Captcha;
            existingSetting.UserId = systemSetting.UserId;
            existingSetting.ThemeId = systemSetting.ThemeId;

            _context.SaveChanges();
            return existingSetting;
        }

        public bool Delete(int id)
        {
            var setting = _context.SystemSettings.Find(id);

            if (setting == null)
            {
                return false;
            }

            _context.SystemSettings.Remove(setting);
            return _context.SaveChanges() > 0;
        }
    }
}