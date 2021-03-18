using NightLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife.Services
{
    public interface INightLifeRepository
    {
        Task<bool> SaveChangesAsync();

        void AddClub(Club club);
        void DeleteClub(Club club);
        Task<List<Club>> GetAllClubs();
        Task<Club> GetClub(int id);

        void AddEvent(Event clubEvent);
        void DeleteEvent(Event clubEvent);
        Task<List<Event>> GetAllClubEvents();
        Task<Event> GetClubEvent(int clubId, int eventId);
    }
}
