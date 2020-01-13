using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    internal class DbDistributor
    {
        /// <summary>
        /// 切换到主库，此方法适用于SQL自动查询服务端版本用
        /// 系统自动会使用 select cast(serverproperty('EngineEdition') as int) 来查询
        /// </summary>
        /// <param name="conn"></param>
        public static void UpdateToMaster(DbConnection conn)
        {
            //UpdateToServer(conn, DbNodeType.Master);
        }
        static void UpdateToServer(DbConnection conn, DbNodeType type)
        {
            //bool exist = false;
            //DbNode node = DbNodeContext.Current.Pop(type, conn.ConnectionString, ref exist);
            //ConsoleHelper.WriteLineWithThreadGreen("数据库服务器切换:" + node.Name + (exist ? "检测到" + type.ToString() + "连接已切换过" : "检测到" + type.ToString() + "连接未切换过"));
            //DbDistributor.UpdateConnectionString(conn, node.ConnectionString);
        }

        internal static void UpdateToSlave(DbConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}
