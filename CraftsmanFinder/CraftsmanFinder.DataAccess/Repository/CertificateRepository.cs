using CraftsmanFinder.DataAccess.Data;
using CraftsmanFinder.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraftsmanFinder.DataAccess.Repository.IRepository
{
    public class CertificateRepository : GenericRepository<Certificate> , ICertificateRepository
    {
        private readonly AppDbContext _context;
        public CertificateRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
