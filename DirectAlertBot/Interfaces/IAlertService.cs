using DirectAlertBot.Entities;

namespace DirectAlertBot.Interfaces
{
    public interface IAlertService
    {
        Alert? FindAlertById(int id);
        void InsertAlert(Alert alert);
        void UpdateAlert(Alert entity);
        void RemoveAlert(int id);

        Client? FindClientByUserId(long id);
        void InsertClient(Client client);
    }
}