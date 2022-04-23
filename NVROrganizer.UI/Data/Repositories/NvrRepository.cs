using NvrOrganizer.DataAccess;
using NvrOrganizer.Model;
using NvrOrganizer.UI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NvrOrganizer.UI.Data.Repositories
{
    public class NvrRepository : INvrRepository
    {

        private NvrOrganizerDbContext _context;

        public NvrRepository(NvrOrganizerDbContext context)
        {
            _context = context;
        }

        public void Add(Nvr nvr)
        {
            _context.Nvrs.Add(nvr);
        }

        public async Task<Nvr> GetByIdAsync(int nvrId)
        {
            return await _context.Nvrs
                .Include(n => n.PhoneNumbers)
                .SingleAsync(n => n.Id == nvrId);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Remove(Nvr model)
        {
            _context.Nvrs.Remove(model);
        }

        public void RemovePhoneNumber(NvrPhoneNumber model)
        {
            _context.NvrPhoneNumbers.Remove(model);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
