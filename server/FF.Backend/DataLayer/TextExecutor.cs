using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace FF.Backend.DataLayer
{
    public class TextExecutor : Executor
    {
        public TextExecutor() : base(CommandType.Text)
        {
        }

        public TextExecutor(string connectionString) :base(CommandType.Text, connectionString)
        {
        }

    }
}
