using Dapper;
using ProjectApi.Models;
using ProjectApi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProjectApi.Repository
{
    public class DevisiRepository : IDevisiRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameters = new DynamicParameters();

        public int Create(Devisi devisi)
        {
            //throw new NotImplementedException();
            var spName = "SP_InsertDevisi";
            parameters.Add("@Name", devisi.Name);
            parameters.Add("@Id", devisi.DepartmentId);
            var create = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            //throw new NotImplementedException();
            var spName = "SP_DeleteDevisi";
            parameters.Add("@Id", Id);
            var del = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return del;
        }

        public IEnumerable<DevisiVM> Get()
        {
            //throw new NotImplementedException();
            var procName = "SP_ViewDevisi";
            var view = connection.Query<DevisiVM>(procName, commandType: CommandType.StoredProcedure);
            return view;
        }

        public async Task<IEnumerable<DevisiVM>> Get(int Id)
        {
            //throw new NotImplementedException();
            var spName = "SP_GetDevisiById";
            parameters.Add("@Id", Id);
            var getDev = await connection.QueryAsync<DevisiVM>(spName, parameters, commandType: CommandType.StoredProcedure);
            return getDev;
        }

        public int Update(int Id, Devisi devisi)
        {
            //throw new NotImplementedException();
            var spName = "SP_UpdateDevisi";
            parameters.Add("@Id", Id);
            parameters.Add("@IdDept", devisi.DepartmentId);
            parameters.Add("@Name", devisi.Name);
            var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}