using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class NotaRepository : GenericRepository<Nota>, INotaRepository
    {
        public NotaRepository(IDbConnection dbConnection) : base(dbConnection)
        {
           
        }
    }
}