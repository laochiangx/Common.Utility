using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    public static class DataTableExtensions
    {
        public static T ToEntity<T>(this DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                    && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);

                        }

                    }
                }
            }

            return entity;
        }

        public static List<T> ToEntities<T>(this DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            if (table == null)
                return null;
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                    && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))//判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }


        // public static dynamic ListToTranslate(this DataTable sender)
        //{
        //    var result = new System.Collections.Generic.List();
        //    if (string.IsNullOrWhiteSpace(sender.Namespace) || string.IsNullOrWhiteSpace(sender.TableName))
        //        throw new Exception("Namespace or TableName is NullOrWhiteSpace");
        //    var typeStr = "";
        //    foreach (DataColumn cloumn in sender.Columns)
        //    {
        //        typeStr += "    public string @2{set;get;}## ##".Replace("@2", cloumn.ColumnName);//Replace("@1", cloumn.DataType.Name).
        //    }
        //    typeStr = "namespace @1##{##  public class @2##  {##@3##  }##}".Replace("@1", sender.Namespace).Replace("@2", sender.TableName).Replace("@3", typeStr).Replace("##", "\r\n");
        //    var cr = new CSharpCodeProvider().CompileAssemblyFromSource(new CompilerParameters(new string[] { "System.dll" }), typeStr);
        //    var type = cr.CompiledAssembly.GetType(string.Format("{0}.{1}", sender.Namespace, sender.TableName));
        //    var properties = type.GetProperties();

        //    foreach (DataRow row in sender.Rows)
        //    {
        //        var dm = Activator.CreateInstance(type);
        //        foreach (DataColumn cloumn in sender.Columns)
        //        {
        //            var property = properties.FirstOrDefault(l => IsEnter(l.Name, cloumn.ColumnName));
        //            if (property != null)
        //            {
        //                property.SetValue(dm, row[cloumn].ToString());
        //            }
        //        }
        //        result.Add(dm);
        //    }
        //    return result;
        //}
    }
}
