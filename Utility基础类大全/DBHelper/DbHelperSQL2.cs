using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace HD.DBHelper
{
	/// <summary>
    /// Enterprise Library 2.0 数据访问进一步封装类
    /// Copyright (C) Maticsoft
	/// All rights reserved	
	/// </summary>
	public abstract class DbHelperSQL2
	{        
		public DbHelperSQL2()
		{
        }

        #region 公用方法

        public static int GetMaxID(string FieldName,string TableName)
        {
            string strSql = "select max(" + FieldName + ")+1 from " + TableName;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);           
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }           
        }
		public static bool Exists(string strSql)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            object obj = db.ExecuteScalar(dbCommand);
			int cmdresult;
			if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
			{
				cmdresult = 0;
			}
			else
			{
				cmdresult = int.Parse(obj.ToString());
			}
			if (cmdresult == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);            
            object obj = db.ExecuteScalar(dbCommand);           
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        /// <summary>
        /// 加载参数
        /// </summary>
        public static void BuildDBParameter(Database db, DbCommand dbCommand, params SqlParameter[] cmdParms)
        {
            foreach (SqlParameter sp in cmdParms)
            {
                db.AddInParameter(dbCommand, sp.ParameterName, sp.DbType,sp.Value);
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="strSql">SQL语句</param>
		/// <returns>影响的记录数</returns>
        public static int ExecuteSql(string strSql)
		{    
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            return db.ExecuteNonQuery(dbCommand);
		}

		public static int ExecuteSqlByTime(string strSql,int Times)
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            dbCommand.CommandTimeout = Times;
            return db.ExecuteNonQuery(dbCommand);
		}
		
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">多条SQL语句</param>		
		public static void ExecuteSqlTran(ArrayList SQLStringList)
		{

            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection dbconn = db.CreateConnection())
            { 
                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    //执行语句
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            DbCommand dbCommand = db.GetSqlStringCommand(strsql);
                            db.ExecuteNonQuery(dbCommand);
                        }
                    }
                    //执行存储过程
                    //db.ExecuteNonQuery(CommandType.StoredProcedure, "InserOrders");
                    //db.ExecuteDataSet(CommandType.StoredProcedure, "UpdateProducts");
                    dbtran.Commit();
                }
                catch
                {
                    dbtran.Rollback();
                }
                finally
                {
                    dbconn.Close();
                }
            }
        }

        #region 执行一个 特殊字段带参数的语句
        /// <summary>
		/// 执行带一个存储过程参数的的SQL语句。
		/// </summary>
		/// <param name="strSql">SQL语句</param>
		/// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string strSql,string content)
		{	
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@content", DbType.String, content);
            return db.ExecuteNonQuery(dbCommand);
		}		
		
        /// <summary>
		/// 执行带一个存储过程参数的的SQL语句。
		/// </summary>
		/// <param name="strSql">SQL语句</param>
		/// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>返回语句里的查询结果</returns>
		public static object ExecuteSqlGet(string strSql,string content)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@content", DbType.String, content);
            object obj = db.ExecuteNonQuery(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }		
		}		
		
        /// <summary>
		/// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
		/// </summary>
		/// <param name="strSql">SQL语句</param>
		/// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSqlInsertImg(string strSql,byte[] fs)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            db.AddInParameter(dbCommand, "@fs", DbType.Byte, fs);
            return db.ExecuteNonQuery(dbCommand);			
        }
        #endregion

        /// <summary>
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="strSql">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
		public static object GetSingle(string strSql)
		{            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }      
		}
		
        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：使用后一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="strSql">查询语句</param>
		/// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSql)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader(dbCommand);
            return dr;      
			
		}		
		
        /// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="strSql">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string strSql)
		{            
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            return db.ExecuteDataSet(dbCommand);
            
		}
		public static DataSet Query(string strSql,int Times)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            dbCommand.CommandTimeout = Times;
            return db.ExecuteDataSet(dbCommand);
		}

		#endregion

		#region 执行带参数的SQL语句

		/// <summary>
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="strSql">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);    
            return db.ExecuteNonQuery(dbCommand);
		}
		
			
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
		public static void ExecuteSqlTran(Hashtable SQLStringList)
		{
            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection dbconn = db.CreateConnection())
            {
                dbconn.Open();
                DbTransaction dbtran = dbconn.BeginTransaction();
                try
                {
                    //执行语句
                    foreach (DictionaryEntry myDE in SQLStringList)
                    {
                        string strsql = myDE.Key.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;                        
                        if (strsql.Trim().Length > 1)
                        {
                            DbCommand dbCommand = db.GetSqlStringCommand(strsql);
                            BuildDBParameter(db, dbCommand, cmdParms);    
                            db.ExecuteNonQuery(dbCommand);
                        }
                    }
                    dbtran.Commit();
                }
                catch
                {
                    dbtran.Rollback();
                }
                finally
                {
                    dbconn.Close();
                }
            }
		}
	
				
		/// <summary>
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="strSql">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
		public static object GetSingle(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);    
            object obj = db.ExecuteScalar(dbCommand);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return null;
            }
            else
            {
                return obj;
            }      
		}
		
		/// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：使用后一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="strSql">查询语句</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader ExecuteReader(string strSql,params SqlParameter[] cmdParms)
		{		
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader(dbCommand);
            return dr;
			
		}		
		
		/// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="strSql">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string strSql,params SqlParameter[] cmdParms)
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(strSql);
            BuildDBParameter(db, dbCommand, cmdParms);   
            return db.ExecuteDataSet(dbCommand);
		}


		private static void PrepareCommand(SqlCommand cmd,SqlConnection conn,SqlTransaction trans, string cmdText, SqlParameter[] cmdParms) 
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
				cmd.Transaction = trans;
			cmd.CommandType = CommandType.Text;//cmdType;
			if (cmdParms != null) 
			{
                foreach (SqlParameter parameter in cmdParms)
				{
					if ( ( parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input ) && 
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					cmd.Parameters.Add(parameter);
				}
			}
		}

		#endregion

		#region 存储过程操作

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>       
        public static int RunProcedure(string storedProcName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            return db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// 执行存储过程，返回输出参数的值和影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="OutParameter">输出参数名称</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static object RunProcedure(string storedProcName, IDataParameter[] InParameters, SqlParameter OutParameter, int rowsAffected)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName);
            BuildDBParameter(db, dbCommand, (SqlParameter[])InParameters);
            db.AddOutParameter(dbCommand, OutParameter.ParameterName, OutParameter.DbType, OutParameter.Size);
            rowsAffected = db.ExecuteNonQuery(dbCommand);
            return db.GetParameterValue(dbCommand,"@" + OutParameter.ParameterName);  //得到输出参数的值
        }

		/// <summary>
        /// 执行存储过程，返回SqlDataReader ( 注意：使用后一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlDataReader</returns>
		public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters )
		{
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);            
            //BuildDBParameter(db, dbCommand, parameters);
            return (SqlDataReader)db.ExecuteReader(dbCommand);           
		}
				
		/// <summary>
        /// 执行存储过程，返回DataSet
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="tableName">DataSet结果中的表名</param>
		/// <returns>DataSet</returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName )
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);
            //BuildDBParameter(db, dbCommand, parameters);
            return db.ExecuteDataSet(dbCommand); 
		}
        /// <summary>
        /// 执行存储过程，返回DataSet(设定等待时间)
        /// </summary>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName ,int Times)
		{           
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand(storedProcName, parameters);
            dbCommand.CommandTimeout = Times;
            //BuildDBParameter(db, dbCommand, parameters);
            return db.ExecuteDataSet(dbCommand); 
		}

		
		/// <summary>
		/// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlCommand</returns>
		private static SqlCommand BuildQueryCommand(SqlConnection connection,string storedProcName, IDataParameter[] parameters)
		{			
			SqlCommand command = new SqlCommand( storedProcName, connection );
			command.CommandType = CommandType.StoredProcedure;
			foreach (SqlParameter parameter in parameters)
			{
				if( parameter != null )
				{
					// 检查未分配值的输出参数,将其分配以DBNull.Value.
					if ( ( parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input ) && 
						(parameter.Value == null))
					{
						parameter.Value = DBNull.Value;
					}
					command.Parameters.Add(parameter);
				}
			}			
			return command;			
		}		
		/// <summary>
		/// 创建 SqlCommand 对象实例(用来返回一个整数值)	
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>SqlCommand 对象实例</returns>
		private static SqlCommand BuildIntCommand(SqlConnection connection,string storedProcName, IDataParameter[] parameters)
		{
			SqlCommand command = BuildQueryCommand(connection,storedProcName, parameters );
			command.Parameters.Add( new SqlParameter ( "ReturnValue",
				SqlDbType.Int,4,ParameterDirection.ReturnValue,
				false,0,0,string.Empty,DataRowVersion.Default,null ));
			return command;
		}
		#endregion	

	}

}
