using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStorage
{
    class DbManager
    {
        private string _tableName;
        private string _connectionString;
        private DbConnection _dbConnection;
        private DbProviderFactory _dbProviderFactory;
        private DbCommand _dbCommand;
        //private DbDataReader _dbDataReader;
        private DbTransaction _dbTransaction;
        private SortedList<string, DbParameter> _dbParameters;
       
        public DbManager(string tableName)
        {
            _tableName = tableName; //проверить существование
            Init();  
            InitParameters();
        }


        private void Init()
        {
            try
            {
                switch (ConfigurationManager.AppSettings["ActiveDatabase"])
                {
                    case "MSSQL":
                        {
                            _connectionString = ConfigurationManager.ConnectionStrings["MSSQLProvider"].ConnectionString;
                            _dbProviderFactory = DbProviderFactories.GetFactory(ConfigurationManager.AppSettings["MSSQLProvider"]);
                            _dbConnection = _dbProviderFactory.CreateConnection();
                            _dbConnection.ConnectionString = _connectionString;
                            _dbCommand = _dbProviderFactory.CreateCommand();
                            _dbCommand.Connection = _dbConnection;   
                            break;
                        }
                    case "MySQL":
                        {

                            break;
                        }
                    case "Oracle":
                        {

                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("Not supported DB");
                        } 
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void AddParameter (string name, DbParameter dbParameter)
        {
            _dbParameters.Add(name, dbParameter);
        }
        public void ClearParameters()
        {
            _dbParameters.Clear();
        }
        protected void Open()
        {
            _dbConnection.Open();
        }
        protected void Close()
        {
            _dbConnection.Close();
        }


        protected int ExecuteQueryWithOutResults(string query)
        {
            Open();
            _dbCommand.CommandText = query;
            int rowsNumber = _dbCommand.ExecuteNonQuery();
            Close();
            return rowsNumber;
        }


        private void InitParameters()
        {
            _dbParameters = new SortedList<string, DbParameter>();
            // DbDataReader dbDataReader = ExecuteQueryWithResults($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{_tableName}' ORDER BY ORDINAL_POSITION");
            Open();
            _dbCommand.CommandText = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{_tableName}' ORDER BY ORDINAL_POSITION";
            DbDataReader dbDataReader = _dbCommand.ExecuteReader();  
            while (dbDataReader.Read())
            {
                DbParameter dbParameter = _dbCommand.CreateParameter();                
                dbParameter.ParameterName = "@" + dbDataReader["COLUMN_NAME"];   // dbParameter.ParameterName = "@id"; 
                string key = dbDataReader["COLUMN_NAME"].ToString(); // string key = "id"; 
                _dbParameters.Add(key, dbParameter);
            }
            Close();

        }


        //protected string getQuery()
        //{

        //}


        //public DbDataReader SelectAll(DbParameter parameter, int offset = 0, int fetch = 100)
        //{
        //    if (!parameter.ParameterName.Contains("@")) throw new ArgumentException("ParameterName invalid");
        //    return ExecuteQueryWithResults($"SELECT * FROM {_tableName} ORDER BY {parameter.ParameterName.Split('@')[1]} LIMIT {offset}, {fetch}");
        //}


        public DbDataReader SelectAll(string orderParameter, int offset, int fetch)//orderParameter - ORDER BY id - сортировка
        {
            if (orderParameter == null)
            {
                orderParameter = _dbParameters.First().Key;//_dbParameters - отсортированный список всех параметров
            }
            if(!_dbParameters.ContainsKey(orderParameter)) throw new ArgumentException("ParameterName invalid");
            string query = $"SELECT * FROM {_tableName} ORDER BY {orderParameter}  OFFSET {offset} ROWS FETCH NEXT {fetch} ROWS ONLY ";  // SELECT * FROM Users ORDER BY id OFFSET 8 ROWS FETCH NEXT 4 ROWS ONLY           
            _dbCommand.CommandText = query;          
            Open();
            DbDataReader reader = _dbCommand.ExecuteReader();
            return reader;
        }

        public int DeleteById(int id)
        {
            if (!_dbParameters.ContainsKey("id")) throw new ArgumentException("Table doesn`t have column id");
            Open();
            _dbCommand.CommandText = $"delete from {_tableName} where id = @id";
            DbParameter idParam = _dbCommand.CreateParameter();
            idParam.DbType = System.Data.DbType.Int32;
            idParam.ParameterName = "@id";
            idParam.Value = id;
            _dbCommand.Parameters.Add(idParam);
            int   result = _dbCommand.ExecuteNonQuery();
            Close();
            return result;
        }

        private DbDataReader ExecuteQueryWithResults(string query)
        {
            Open();          
            _dbCommand.CommandText = query;
            Console.WriteLine(query);
            DbDataReader reader = _dbCommand.ExecuteReader();
           Close();
            return reader;
        }



    }
}
