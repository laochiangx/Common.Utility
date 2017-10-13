using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

using IBatisNet.Common;
using IBatisNet.Common.Pagination;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Configuration;

namespace HD.DBHelper
{
    /// <summary>
    /// 基于IBatisNet的数据访问基类 
    /// </summary>
    public class BaseSqlMapDao 
    {
        private ISqlMapper sqlMap;
        public BaseSqlMapDao()
        {
            //DomSqlMapBuilder builder = new DomSqlMapBuilder(true);
            //sqlMap = builder.Configure();

            Assembly assembly = Assembly.Load("IBatisNetDemo");
            Stream stream = assembly.GetManifestResourceStream("IBatisNetDemo.sqlmap.config");

            DomSqlMapBuilder builder = new DomSqlMapBuilder();            
            sqlMap = builder.Configure(stream);
        }

        ///// <summary>
        ///// IsqlMapper实例
        ///// </summary>
        ///// <returns></returns>
        //public static ISqlMapper sqlMap = (ContainerAccessorUtil.GetContainer())["sqlServerSqlMap"] as ISqlMapper;

        //public SqlMapper SqlMap
        //{
        //    get
        //    {
        //        Assembly assembly = Assembly.Load("IBatisNetDemo");
        //        Stream stream = assembly.GetManifestResourceStream("IBatisNetDemo.sqlmap.config");

        //        DomSqlMapBuilder builder = new DomSqlMapBuilder();
        //        builder.
        //        SqlMapper sqlMap = builder.Configure(stream);
        //    }
        //}

        /// <summary>
        /// 得到列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="statementName">操作名称，对应xml中的Statement的id</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject)
        {
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 得到指定数量的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statementName"></param>
        /// <param name="parameterObject">参数</param>
        /// <param name="skipResults">跳过的记录数</param>
        /// <param name="maxResults">最大返回的记录数</param>
        /// <returns></returns>
        protected IList<T> ExecuteQueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            try
            {
                return sqlMap.QueryForList<T>(statementName, parameterObject, skipResults, maxResults);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 得到分页的列表
        /// </summary>
        /// <param name="statementName">操作名称</param>
        /// <param name="parameterObject">参数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        protected IPaginatedList ExecuteQueryForPaginatedList(string statementName, object parameterObject, int pageSize)
        {
            try
            {
                return sqlMap.QueryForPaginatedList(statementName, parameterObject, pageSize);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for paginated list.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 查询得到对象的一个实例
        /// </summary>
        /// <typeparam name="T">对象type</typeparam>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        /// <returns></returns>
        protected T ExecuteQueryForObject<T>(string statementName, object parameterObject)
        {
            try
            {
                return sqlMap.QueryForObject<T>(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for object.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 执行添加
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteInsert(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Insert(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for insert.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteUpdate(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Update(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for update.  Cause: " + e.Message, e);
            }
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="statementName">操作名</param>
        /// <param name="parameterObject">参数</param>
        protected void ExecuteDelete(string statementName, object parameterObject)
        {
            try
            {
                sqlMap.Delete(statementName, parameterObject);
            }
            catch (Exception e)
            {
                throw new DataMapperException("Error executing query '" + statementName + "' for delete.  Cause: " + e.Message, e);
            }
        }

        ///// <summary>
        ///// 得到流水号
        ///// </summary>
        ///// <param name="tableName">表名</param>
        ///// <returns></returns>
        //public int GetId(string tableName)
        //{
        //    try
        //    {
        //        Stream stream = sqlMap.QueryForObject("GetStreamId", tableName) as Stream;
        //        return stream.IMaxID;
        //    }
        //    catch (Exception e)
        //    {
        //        throw (e);
        //    }
        //}

    }


    ///// <summary>
    ///// Stream 
    ///// </summary>
    //public class Stream
    //{
    //    public Stream()
    //    {

    //    }

    //    private string ctablename;

    //    public string CTableName
    //    {
    //        get { return ctablename; }
    //        set { ctablename = value; }
    //    }
    //    private int imaxid;

    //    public int IMaxID
    //    {
    //        get { return imaxid; }
    //        set { imaxid = value; }
    //    }
    //}

    //public class ContainerAccessorUtil
    //{
    //    private ContainerAccessorUtil()
    //    {
    //    }

    //    /// <summary>
    //    /// Obtain the Cuyahoga container.
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IWindsorContainer GetContainer()
    //    {
    //        IContainerAccessor containerAccessor = HttpContext.Current.ApplicationInstance as IContainerAccessor;

    //        if (containerAccessor == null)
    //        {
    //            throw new Exception("You must extend the HttpApplication in your web project " +
    //                "and implement the IContainerAccessor to properly expose your container instance");
    //        }

    //        IWindsorContainer container = containerAccessor.Container as IWindsorContainer;

    //        if (container == null)
    //        {
    //            throw new Exception("The container seems to be unavailable in " +
    //                "your HttpApplication subclass");
    //        }

    //        return container;
    //    }
    //} 


}
