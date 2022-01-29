using DirectAlertBot.Data;
using System.Linq;
using DirectAlertBot.Interfaces;
using DirectAlertBot.Entities;

namespace DirectAlertBot.Services
{
    public class AlertService : IAlertService
    {
        private readonly DataContext _context;

        public AlertService(DataContext context)
        {
            _context = context;
        }

        public void InsertAlert(Alert alert)
        {
            _context.Alerts.Add(alert);
            _context.SaveChanges();
        }

        public Alert? FindAlertById(int id)
        {
            return _context.Alerts.Find(id);
        }

        public void UpdateAlert(Alert entity)
        {
            _context.Alerts.Update(entity);
            _context.SaveChanges();
        }

        public void RemoveAlert(int id)
        {
            Alert? obj = FindAlertById(id);
            if (obj == null) 
                return;
            _context.Alerts.Remove(obj);
            _context.SaveChanges();
        }

        public Client? FindClientByUserId(long userId)
        {
            return _context.Clients.FirstOrDefault(x => x.UserId == userId);
        }

        public void InsertClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }
    }
}
