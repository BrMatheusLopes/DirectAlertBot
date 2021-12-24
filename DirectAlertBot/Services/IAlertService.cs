using DirectAlertBot.Models;

namespace DirectAlertBot.Services
{
    public interface IAlertService
    {
        Alert FindById(int id);
        void Insert(Alert alert);
        void Remove(int id);
    }
}