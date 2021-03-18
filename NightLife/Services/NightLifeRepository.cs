using Microsoft.EntityFrameworkCore;
using NightLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife.Services
{
    public class NightLifeRepository : INightLifeRepository
    {
        private readonly NightLifeApiContext _context;

        public NightLifeRepository(NightLifeApiContext context)
        {
            _context = context;
        }

        public void AddClub(Club club)
        {
            _context.Clubs.Add(club);
        }

        public void AddEvent(Event clubEvent)
        {
            _context.Events.Add(clubEvent);
        }

        public void DeleteClub(Club club)
        {
            _context.Clubs.Remove(club);
        }

        public void DeleteEvent(Event clubEvent)
        {
            _context.Events.Remove(clubEvent);
        }

        public async Task<List<Event>> GetAllClubEvents()
        {
            IQueryable<Event> query = _context.Events.Include(e => e.Club);

            return await query.ToListAsync();
        }

        public async Task<List<Club>> GetAllClubs()
        {
            IQueryable<Club> query = _context.Clubs.Include(c => c.Events);

            return await query.ToListAsync();
        }

        public async Task<Club> GetClub(int id)
        {
            IQueryable<Club> query = _context.Clubs.Include(c => c.Events);

            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Event> GetClubEvent(int clubId, int eventId)
        {
            IQueryable<Event> query = _context.Events.Include(e => e.Club);

            query = query.Where(e => e.ClubId == clubId && e.Id == eventId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
