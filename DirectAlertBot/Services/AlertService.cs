using DirectAlertBot.Data;
using DirectAlertBot.Models;

namespace DirectAlertBot.Services
{
    public class AlertService : IAlertService
    {
        private readonly DataContext _context;

        public AlertService(DataContext context)
        {
            _context = context;
        }

        public void Insert(Alert alert)
        {
            _context.Alerts.Add(alert);
            _context.SaveChanges();
        }

        public Alert FindById(int id)
        {
            return _context.Alerts.Find(id);
        }

        public void Remove(int id)
        {
            Alert obj = FindById(id);
            _context.Alerts.Remove(obj);
            _context.SaveChanges();
        }
    }
}
