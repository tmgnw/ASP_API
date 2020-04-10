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
    public class DepartmentRepository : IDepartmentRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameters = new DynamicParameters();

        public int Create(Department department)
        {
            //throw new NotImplementedException();
            var spName = "SP_InsertDepartment";
            parameters.Add("@Name", department.Name);
            var create = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            //throw new NotImplementedException();
            var spName = "SP_DeleteDepartment";
            parameters.Add("@id", Id);
            var del = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return del;
        }

        public IEnumerable<Department> Get()
        {
            //throw new NotImplementedException();
            var procName = "SP_ViewDepartment";
            var view = connection.Query<Department>(procName, commandType: CommandType.StoredProcedure);
            return view;
        }

        public async Task<IEnumerable<Department>> Get(int Id)
        {
            //throw new NotImplementedException();
            var spName = "SP_GetDepartmentById";
            parameters.Add("@Id", Id);
            var getDept = await connection.QueryAsync<Department>(spName, parameters, commandType: CommandType.StoredProcedure);
            return getDept;
        }

        public int Update(int Id, Department department)
        {
            //throw new NotImplementedException();
            var spName = "SP_UpdateDepartment";
            parameters.Add("@newId", Id);
            parameters.Add("@Name", department.Name);
            var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}