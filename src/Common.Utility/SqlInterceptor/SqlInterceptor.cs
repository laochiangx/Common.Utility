using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    /// <summary>
    /// SQL语句拦截器
    /// </summary>
    public class SqlInterceptor
    {
        public void Intercept(DbTransaction trans, DbConnection connection, string cmdText, CommandType cmdType)
        {
            //判断是否启用了读写分离功能.
            //if (!DbNodeContext.Current.EnableReadWrite)
            //{
            //    return;
            //}

            if (string.IsNullOrEmpty(cmdText))
            {
                return;
            }
            cmdText = cmdText.Trim().TrimStart('\r').TrimStart('\n');
            if (
                cmdText.IndexOf("UPDATE", StringComparison.OrdinalIgnoreCase) >= 0
                || cmdText.IndexOf("DELETE", StringComparison.OrdinalIgnoreCase) >= 0
                || cmdText.IndexOf("INSERT", StringComparison.OrdinalIgnoreCase) >= 0
                || trans != null
                || cmdType == CommandType.StoredProcedure
               )
            {
                DbDistributor.UpdateToMaster(connection);
            }
            else
            {
                DbDistributor.UpdateToSlave(connection);
            }
        }
    }
}
