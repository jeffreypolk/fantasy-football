using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace FF.Backend.DataLayer
{
    public abstract class Executor
    {
        protected List<SqlParam> sqlParams = new List<SqlParam>();
        private CommandType CommandType;

        public string ConnectionString { get; set; }

        public Executor(System.Data.CommandType commandType)
        {
            CommandType = commandType;
        }

        public Executor(System.Data.CommandType commandType, string connectionString)
        {
            ConnectionString = connectionString;
            CommandType = commandType;
        }

        public void Execute(string sql)
        {
            SqlConnection cnn = null;

            try
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.CommandType = CommandType;
                    foreach (var p in sqlParams)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
        }

        public DataTable ExecuteDataTable(string sql)
        {
            DataTable ret;
            SqlConnection cnn = null;

            try
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.CommandType = CommandType;
                    foreach (var p in sqlParams)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        ret = new DataTable();
                        ret.Load(dr);
                    }
                }
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return ret;
        }

        public DataSet ExecuteDataSet(string sql)
        {
            var ret = new DataSet();
            SqlConnection cnn = null;

            try
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.CommandType = CommandType;
                    foreach (var p in sqlParams)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ret);
                    }
                }
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return ret;
        }

        public string ExecuteScalarString(string sql)
        {
            return ExecuteScalar(sql).ToString();
        }

        public int ExecuteScalarInt(string sql)
        {
            return int.Parse(ExecuteScalarString(sql));
        }

        private object ExecuteScalar(string sql)
        {
            object ret;
            SqlConnection cnn = null;

            try
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.CommandType = CommandType;
                    foreach (var p in sqlParams)
                    {
                        cmd.Parameters.AddWithValue(p.Name, p.Value);
                    }
                    ret = cmd.ExecuteScalar();
                }
            }
            finally
            {
                if (cnn != null)
                {
                    cnn.Close();
                    cnn.Dispose();
                }
            }
            return ret;
        }

        public void AddParam(string name, object value)
        {
            sqlParams.Add(new SqlParam() { Name = name, Value = value });
        }

        public void ClearParams()
        {
            sqlParams.Clear();
        }
    }
}
