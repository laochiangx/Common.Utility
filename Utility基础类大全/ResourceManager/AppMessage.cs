//------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2010 , Jirisoft , Ltd. 
//------------------------------------------------------------

using System;
using System.Globalization;

namespace DotNet.Utilities
{
    /// <summary>
    ///	BUBaseAppMessage
    /// 通用消息控制基类
    /// 
    /// 修改纪录
    ///		2007.05.17 版本：1.0	JiRiGaLa 建立，为了提高效率分开建立了类。
    ///	
    /// 版本：3.1
    ///
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2007.05.17</date>
    /// </author> 
    /// </summary>
    public class AppMessage
    {
        /// <summary>
        /// 提示信息.
        /// </summary>
        public static string MSG0000 = "提示信息";

        /// <summary>
        /// 发生未知错误.
        /// </summary>
        public static string MSG0001 = "发生未知错误。";

        /// <summary>
        /// 数据库联接不正常.
        /// </summary>
        public static string MSG0002 = "数据库联接不正常。";

        /// <summary>
        /// WebService联接不正常.
        /// </summary>
        public static string MSG0003 = "WebService 联接不正常。";

        /// <summary>
        /// 任何数据未被修改.
        /// </summary>
        public static string MSG0004 = "任何数据未被修改。";

        /// <summary>
        /// 记录未找到,可能已被其他人删除.
        /// </summary>
        public static string MSG0005 = "记录未找到，可能已被其他人删除。";

        /// <summary>
        /// 数据已被其他人修改,请按F5键,重新刷新获得数据.
        /// </summary>
        public static string MSG0006 = "数据已被其他人修改,请按F5键,重新刷新获得数据。";

        /// <summary>
        /// '{O}'不允许为空,请输入.
        /// </summary>
        public static string MSG0007 = "{0} 不允许为空，请输入。";

        /// <summary>
        /// {0} 已存在,不可以重复.
        /// </summary>
        public static string MSG0008 = "{0} 已存在，不可以重复。";

        /// <summary>
        /// 新增成功.
        /// </summary>
        public static string MSG0009 = "新增成功。";

        /// <summary>
        /// 更新成功.
        /// </summary>
        public static string MSG0010 = "更新成功。";

        /// <summary>
        /// 保存成功.
        /// </summary>
        public static string MSG0011 = "保存成功。";

        /// <summary>
        /// 批量保存成功.
        /// </summary>
        public static string MSG0012 = "批量保存成功。";

        /// <summary>
        /// 删除成功.
        /// </summary>
        public static string MSG0013 = "删除成功。";

        /// <summary>
        /// 批量删除成功.
        /// </summary>
        public static string MSG0014 = "批量删除成功。";

        /// <summary>
        /// 您确认删除吗?
        /// </summary>
        public static string MSG0015 = "您确认删除吗？";

        /// <summary>
        /// 您确认删除 '{0}'吗?
        /// </summary>
        public static string MSG0016 = "您确认删除 {0} 吗？";

        /// <summary>
        /// 当前记录不允许被删除.
        /// </summary>
        public static string MSG0017 = "当前记录不允许被删除。";

        /// <summary>
        /// 当前记录 '{0}' 不允许被删除.
        /// </summary>
        public static string MSG0018 = "当前记录 {0} 不允许被删除。";

        /// <summary>
        /// 当前记录不允许被编辑,请按F5键,重新获取数据最新数据.
        /// </summary>
        public static string MSG0019 = "当前记录不允许被编辑，请按F5键,重新获取数据最新数据。";

        /// <summary>
        /// 当前记录 '{0}' 不允许被编辑,请按F5键,重新获取数据最新数据.
        /// </summary>
        public static string MSG0020 = "当前记录 {0} 不允许被编辑，请按F5键，重新获取数据最新数据。";

        /// <summary>
        /// 当前记录已是第一条记录.
        /// </summary>
        public static string MSG0021 = "当前记录已是第一条记录。";

        /// <summary>
        /// 当前记录已是最后一条记录.
        /// </summary>
        public static string MSG0022 = "当前记录已是最后一条记录。";

        /// <summary>
        /// 请至少选择一项.
        /// </summary>
        public static string MSG0023 = "请选择一条记录。";

        /// <summary>
        /// 请至少选择一项 '{0}'.
        /// </summary>
        public static string MSG0024 = "请至少选择一条记录。";

        /// <summary>
        /// '{0}'不能大于'{1}'.
        /// </summary>
        public static string MSG0025 = "{0} 不能大于{1}。";

        /// <summary>
        /// '{0}'不能小于'{1}'.
        /// </summary>
        public static string MSG0026 = "{0} 不能小于 {1}。";

        /// <summary>
        /// '{0}'不能等于'{1}'.
        /// </summary>
        public static string MSG0027 = "{0} 不能等于 {1}。";

        /// <summary>
        /// 输入的'{0}'不是有效的日期.
        /// </summary>
        public static string MSG0028 = "输入的 {0} 不是有效的日期。";

        /// <summary>
        /// 输入的'{0}'不是有效的字符.
        /// </summary>
        public static string MSG0029 = "输入的 {0} 不是有效的字符。";

        /// <summary>
        /// 输入的'{0}'不是有效的数字.
        /// </summary>
        public static string MSG0030 = "输入的 {0} 不是有效的数字。";

        /// <summary>
        /// 输入的'{0}'不是有效的金额.
        /// </summary>
        public static string MSG0031 = "输入的 {0} 不是有效的金额。";

        /// <summary>
        /// '{0}'名不能包含：\ / : * ? " < > |
        /// </summary>
        public static string MSG0032 = "{0} 名包含非法字符。";

        /// <summary>
        /// 数据已经被引用,有关联数据在
        /// </summary>
        public static string MSG0033 = "数据已经被引用，有关联数据在。";

        /// <summary>
        /// 数据已经被引用,有关联数据在.是否强制删除数据?
        /// </summary>
        public static string MSG0034 = "数据已经被引用，有关联数据在，是否强制删除数据？";

        /// <summary>
        /// {0} 有子节点不允许被删除.
        /// </summary>
        public static string MSG0035 = "{0} 有子节点不允许被删除，有子节点还未被删除。";

        /// <summary>
        /// {0} 不能移动到 {1}.
        /// </summary>
        public static string MSG0036 = "{0} 不能移动到 {1}。";

        /// <summary>
        /// {0} 下的子节点不能移动到 {1}.
        /// </summary>
        public static string MSG0037 = "{0} 下的子节点不能移动到 {1}。";

        /// <summary>
        /// 确认移动 {0} 到 {1} 吗?
        /// </summary>
        public static string MSG0038 = "确认移动 {0} 到 {1} 吗？";

        /// <summary>
        /// '{0}'不等于'{1}'.
        /// </summary>
        public static string MSG0039 = "{0} 不等于 {1}。";

        /// <summary>
        /// {0} 错误.
        /// </summary>
        public static string MSG0040 = "{0} 错误。";

        /// <summary>
        /// 确认审核通过吗?.
        /// </summary>
        public static string MSG0041 = "确认审核通过吗？";

        /// <summary>
        /// 确认驳回吗?.
        /// </summary>
        public static string MSG0042 = "确认审核驳回吗？";

        /// <summary>
        /// 成功锁定数据.
        /// </summary>
        public static string MSG0043 = "不能锁定数据。";

        /// <summary>
        /// 不能锁定数据.
        /// </summary>
        public static string MSG0044 = "成功锁定数据。";

        /// <summary>
        /// 数据被修改提示
        /// </summary>
        public static string MSG0045 = "数据已经改变，想保存数据吗？";

        /// <summary>
        /// 最近 {0} 次内密码不能重复。
        /// </summary>
        public static string MSG0046 = "最近 {0} 次内密码不能重复。";

        /// <summary>
        /// 密码已过期，账户被锁定，请联系系统管理员。
        /// </summary>
        public static string MSG0047 = "密码已过期，账户被锁定，请联系系统管理员。";

        /// <summary>
        /// 数据已经改变，不保存数据？
        /// </summary>
        public static string MSG0065 = "数据已经改变，不保存数据？";

        public static string MSG0048 = "拒绝登录，用户已经在线上。";
        public static string MSG0049 = "拒绝登录，网卡Mac地址不符限制条件。";
        public static string MSG0050 = "拒绝登录，IP地址不符限制条件";
        public static string MSG0051 = "已到在线用户最大数量限制。";


        public static string MSG0060 = "请先创建该职员的登录系统的用户信息。";

        /// <summary>
        /// 您确认移除吗?
        /// </summary>
        public static string MSG0075 = "您确认移除吗？";

        /// <summary>
        /// 您确认移除 '{0}'吗?
        /// </summary>
        public static string MSG0076 = "您确认移除 {0} 吗？";

        public static string MSG0700 = "已经成功连接到目标数据。";

        public static string MSG9800 = "值";
        public static string MSG9900 = "公司";
        public static string MSG9901 = "部门";
        public static string MSG9956 = "未找到满足条件的记录。";
        public static string MSG9957 = "用户名";
        public static string MSG9958 = "数据验证错误。";
        public static string MSG9959 = "新密码";
        public static string MSG9960 = "确认密码";
        public static string MSG9961 = "原密码";
        public static string MSG9962 = "修改 {0} 成功。";
        public static string MSG9963 = "设置 {0} 成功。";
        public static string MSG9964 = "密码";
        public static string MSG9965 = "登录成功。";
        public static string MSG9966 = "用户没有找到，请注意大小写。";
        public static string MSG9967 = "密码错误，请注意大小写。";
        public static string MSG9968 = "登录被拒绝，请与管理员联系。";
        public static string MSG9969 = "基础编码";
        public static string MSG9970 = "职员";
        public static string MSG9971 = "组织机构";
        public static string MSG9972 = "角色";
        public static string MSG9973 = "模块";
        public static string MSG9974 = "文件夹";
        public static string MSG9975 = "权限";
        public static string MSG9976 = "代码";
        public static string MSG9977 = "编号";
        public static string MSG9978 = "名称";
        public static string MSG9979 = "父节点代码";
        public static string MSG9980 = "父节点名称";
        public static string MSG9981 = "功能分类代码";
        public static string MSG9982 = "唯一识别代码";
        public static string MSG9983 = "主题";
        public static string MSG9984 = "内容";
        public static string MSG9985 = "状态码";
        public static string MSG9986 = "次数";
        public static string MSG9987 = "有效";
        public static string MSG9988 = "备注";
        public static string MSG9989 = "排序码";
        public static string MSG9990 = "创建者代码";
        public static string MSG9991 = "创建时间";
        public static string MSG9992 = "最后修改者代码";
        public static string MSG9993 = "最后修改时间";
        public static string MSG9994 = "排序";
        public static string MSG9995 = "代码";
        public static string MSG9996 = "索引";
        public static string MSG9997 = "字段";
        public static string MSG9998 = "表";
        public static string MSG9999 = "数据库";

        #region public static int GetLanguageResource() 从当前指定的语言包读取信息
        /// <summary>
        /// 从当前指定的语言包读取信息
        /// </summary>
        /// <returns></returns>
        //public static int GetLanguageResource()
        //{
        //    AppMessage AppMessage = new AppMessage();
        //    return BaseInterfaceLogic.GetLanguageResource(AppMessage);
        //}
        #endregion

        #region public static string Format(string value, params string[] messages) 格式化一个资源字符串
        /// <summary>
        /// 格式化一个资源字符串
        /// </summary>
        /// <param name="value">目标字符串</param>
        /// <param name="messages">传入的信息</param>
        /// <returns>字符串</returns>
        public static string Format(string value, params string[] messages)
        {
            return String.Format(CultureInfo.CurrentCulture, value, messages);
        }
        #endregion

        #region public static string GetMessage(string id) 读取一个资源定义
        /// <summary>
        /// 读取一个资源定义
        /// </summary>
        /// <param name="id">资源代码</param>
        /// <returns>字符串</returns>
        public static string GetMessage(string id)
        {
            string returnValue = string.Empty;
            returnValue = ResourceManagerWrapper.Instance.Get(id);
            return returnValue;
        }
        #endregion

        #region public static string GetMessage(string id, params string[] messages)
        /// <summary>
        /// 读取一个资源定义
        /// </summary>
        /// <param name="id">资源代码</param>
        /// <param name="messages">传入的信息</param>
        /// <returns>字符串</returns>
        public static string GetMessage(string id, params string[] messages)
        {
            string returnValue = string.Empty;
            returnValue = ResourceManagerWrapper.Instance.Get(id);
            returnValue = String.Format(CultureInfo.CurrentCulture, returnValue, messages);
            return returnValue;
        }
        #endregion
    }
}