using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApi.Repository.Interface
{
    interface IDevisiRepository
    {
        IEnumerable<DevisiVM> Get();
        Task<IEnumerable<DevisiVM>> Get(int Id);
        int Create(Devisi devisi);
        int Update(int Id, Devisi devisi);
        int Delete(int Id);
    }
}
