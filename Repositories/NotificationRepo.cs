using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class NotificationRepo
    {
        private readonly isc_dbContext _context;

        public NotificationRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Notification> GetNotifications()
        {
            return _context.Notifications.ToList();
        }

        public Notification GetNotificationById(long id)
        {
            return _context.Notifications.FirstOrDefault(n => n.Id == id);
        }

        public Notification CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
            return notification;
        }

        public Notification UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return notification;
        }

        public bool DeleteNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
            return true;
        }
    }
}
