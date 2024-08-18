using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace FF.Backend.DataLayer
{
    public class StoredProcedureExecutor : Executor
    {
        public StoredProcedureExecutor() : base(CommandType.StoredProcedure)
        {
        }

        public StoredProcedureExecutor(string connectionString) : base(CommandType.StoredProcedure, connectionString)
        {
        }
    }
}
