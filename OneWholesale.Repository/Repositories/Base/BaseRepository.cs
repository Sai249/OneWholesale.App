using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneWholesale.Repository.DbFactory;

namespace OneWholesale.Repository.Repositories.Base
{
    public class BaseRepository : IBaseRepository
    {
        protected IDbConnection _connection;
        private bool _disposed;
        private BaseRepository(IDbOWConnection dbOWConnection)
        {
            try
            {
                _connection = dbOWConnection.GetConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection?.Dispose();
                }
                _disposed = true;

            }
        }
        ~BaseRepository()
        {
            Dispose(false);
        }
    }
}
