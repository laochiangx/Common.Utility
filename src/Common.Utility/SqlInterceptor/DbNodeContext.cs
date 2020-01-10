using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    //internal class DbNodeContext
    //{
    //    private DbGroupConfiguration _dbGroupConfiguration = DbGroupConfiguration.Instance.Current;
    //    private Lazy<DefaultWeight<DbNode>> _masterWeight = null;     //主服务器权重
    //    private Lazy<DefaultWeight<DbNode>> _slaveWeight = null;    //从服务器权重


    //    public static DbNodeContext Current = new DbNodeContext();

    //    public DbNodeContext()
    //    {
    //        _masterWeight = new Lazy<DefaultWeight<DbNode>>(() => new DefaultWeight<DbNode>(GetOnline(DbNodeType.Master)));
    //        _slaveWeight = new Lazy<DefaultWeight<DbNode>>(() => new DefaultWeight<DbNode>(GetOnline(DbNodeType.Slave)));
    //    }

    //    #region EnableReadWrite 是否启用读写分离
    //    /// <summary>
    //    /// 是否启用读写分离,判断条件就是是否有读写分离文件
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool EnableReadWrite
    //    {
    //        get
    //        {
    //            bool result = false;
    //            result = _dbGroupConfiguration != null;
    //            if (result)
    //            {
    //                result = _dbGroupConfiguration.DbGroup.DbNodes.Count > 0;
    //            }
    //            return result;
    //        }
    //    }
    //    #endregion


    //    #region ResetWeightList 权限数据重新设置
    //    public void ResetWeightList()
    //    {
    //        _masterWeight.Value.ResetWeightList(GetOnline(DbNodeType.Master));
    //        _slaveWeight.Value.ResetWeightList(GetOnline(DbNodeType.Slave));
    //    }
    //    #endregion

    //    #region UpdateNodeState 更新节点状态
    //    public void UpdateNodeState(DbNode newNode)
    //    {
    //        var node = _dbGroupConfiguration.DbGroup.DbNodes.Where(n => n.NodeType == newNode.NodeType
    //                                                       && n.ConnectionString == newNode.ConnectionString
    //                                                       && n.ProviderName == newNode.ProviderName
    //                                                   ).FirstOrDefault();
    //        //改变节点状态
    //        if (node != null)
    //        {
    //            node.State = newNode.State;
    //        }
    //    }
    //    #endregion



    //    #region NotifyExecute 通知执行成功
    //    public void NotifyExecute(string connectionString)
    //    {
    //        var nodes = _dbGroupConfiguration.DbGroup
    //                                        .DbNodes.Where(t => t.ConnectionString == connectionString)
    //                                        .ToList();

    //        foreach (DbNode node in nodes)
    //        {
    //            node.LastModifyTime = DateTime.Now;
    //            // ConsoleHelper.WriteLineWithThreadGreen("执行成功,更新最后连接时间为:"+node.LastModifyTime.ToString("yyyy-MM-dd HH:mm:ss"));
    //        }

    //    }
    //    #endregion

    //    #region Pop 随机取主从节点 
    //    public DbNode Pop(DbNodeType nodeType, string connectionString, ref bool exist)
    //    {
    //        if (nodeType == DbNodeType.Master)
    //        {
    //            return PopMaster(connectionString, ref exist);
    //        }
    //        else
    //        {
    //            return PopSlave(connectionString, ref exist);
    //        }
    //    }

    //    private DbNode PopMaster(string connectionString, ref bool exist)
    //    {
    //        //先检查该连接字符串是否已经在库中，如果存在说明已经在EFStore中已经处理
    //        DbNode existNode = GetByConnectionString(DbNodeType.Master, connectionString);
    //        if (existNode != null)
    //        {
    //            //ConsoleHelper.WriteLineWithThreadGreen("检测到主库连接已切换过.");
    //            exist = true;
    //            return existNode;
    //        }

    //        return _masterWeight.Value
    //                 .GetWeightList(1)
    //                 .FirstOrDefault();
    //    }
    //    private DbNode PopSlave(string connectionString, ref bool exist)
    //    {
    //        DbNode existNode = GetByConnectionString(DbNodeType.Slave, connectionString);
    //        if (existNode != null)
    //        {
    //            //ConsoleHelper.WriteLineWithThreadGreen("检测到从库连接已切换过.");
    //            exist = true;
    //            return existNode;
    //        }
    //        return _slaveWeight.Value
    //                 .GetWeightList(1)
    //                 .FirstOrDefault();
    //    }

    //    #endregion

    //    #region IsConnectionExist 检查当前指定的连接字符串是否存在主/从库中
    //    public bool IsConnectionExist(string connectionString)
    //    {

    //        DbNode existNode = _dbGroupConfiguration.DbGroup
    //                                                .DbNodes
    //                                                .FirstOrDefault(t => t.State == DbNodeState.Online
    //                                                                    && t.ConnectionString == connectionString);
    //        return existNode != null;
    //    }
    //    #endregion

    //    #region GetByConnectionString 根据连接字符串获取节点
    //    private DbNode GetByConnectionString(DbNodeType nodeType, string connectionString)
    //    {
    //        List<DbNode> nodes = GetOnline(nodeType);
    //        DbNode existNode = nodes.FirstOrDefault(t => t.ConnectionString == connectionString);
    //        return existNode;
    //    }
    //    #endregion

    //    #region GetOnline 获取可用的服务器节点
    //    private List<DbNode> GetOnline(DbNodeType nodeType)
    //    {
    //        var result = _dbGroupConfiguration.DbGroup
    //                        .DbNodes.Where(t => t.NodeType == nodeType && t.State == DbNodeState.Online)
    //                        .ToList();
    //        return result;
    //    }
    //    #endregion

    //}
}
