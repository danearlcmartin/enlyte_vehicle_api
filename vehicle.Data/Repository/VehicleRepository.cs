using Common;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using Utils;
using vehicle.Data.IRepository;
using vehicle.Data.Models;

namespace vehicle.Data
{
    public class VehicleRepository: IVehicleRepository
    {
        private readonly SQLDataProvider _sQLDataProvider;

        public VehicleRepository(SQLDataProvider sQLDataProvider) { _sQLDataProvider = sQLDataProvider; }
        public JArray GetVehicle(int Id)
        {
            DataSet ds = _sQLDataProvider.ExecuteStoredProcedure("dob.GetVehicle", CommandType.StoredProcedure,
                new SqlParameter[] { new SqlParameter("@p_Id", Id) });

            return ds.Tables[0].ToJArray();
        }

        public DataSet AddUpdateVehicle(Vehicle vehicle)
        {
            DataSet ds = _sQLDataProvider.ExecuteStoredProcedure("dbo.AddUpdateVehicle",CommandType.StoredProcedure, new SqlParameter[]
            {
                new SqlParameter("@p_Id",vehicle.Id),
                new SqlParameter("@p_Year",vehicle.Year),
                new SqlParameter("@p_Make",vehicle.Make),
                new SqlParameter("@p_Year",vehicle.Year)
            });

            return ds;
        }

        public DataSet Delete(int Id)
        {
            DataSet ds = _sQLDataProvider.ExecuteStoredProcedure("dbo.DeleteVehicle",CommandType.StoredProcedure, new SqlParameter[]
            {
                new SqlParameter("@p_Id",Id),
            });

            return ds;
        }
    }
}
