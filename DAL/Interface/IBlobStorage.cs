using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IBlobStorage
    {
        Task<string> CreateFile(string file, string fileName);
        Task<bool> DeleteBlobFromServer(string fileName);
        Task<string> GetBlobFromServer(string fileName);
    }
}
