using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
/// <summary>
/// 处理枚举类
/// </summary>
public class RenumDropList
    {
       
     /// <summary>
        /// 讲枚举绑定在DropDownList上
     /// </summary>
     /// <param name="list">类型</param>
     /// <param name="text">第一项文字</param>
     /// <param name="TypeEnum">枚举(默认第一项为0)</param>
       public static void BindDropDownList(DropDownList list,string text,Type TypeEnum)
       {
           List<ListItem> TypeList = new List<ListItem>();

          // TypeList.Add(new ListItem(text, value));
           TypeList.Add(new ListItem { Text = ""+text+"", Value = "0"});


           foreach (object type in Enum.GetValues(TypeEnum))
            {
                TypeList.Add(new ListItem(type.ToString(), ((int)type).ToString()));
            }
            list.DataSource = TypeList;
            list.DataTextField = "text";
            list.DataValueField = "value";
            list.DataBind();

       }
      /// <summary>
       /// 讲枚举绑定在DropDownList上
      /// </summary>
      /// <param name="list">控件</param>
      /// <param name="TypeEnum">枚举</param>
       public static void BindDropDownList(DropDownList list, Type TypeEnum)
       {
           List<ListItem> TypeList = new List<ListItem>();
           foreach (object type in Enum.GetValues(TypeEnum))
           {
               TypeList.Add(new ListItem(type.ToString(), ((int)type).ToString()));
           }
           list.DataSource = TypeList;
           list.DataTextField = "text";
           list.DataValueField = "value";
           list.DataBind();

       }
       /// <summary>
       /// 讲枚举绑定在RadioButtonList上面
       /// </summary>
       /// <param name="list"></param>
       public static void BindRadioButtonList(RadioButtonList list,Type TypeEnem)
       {
           List<ListItem> TypeList = new List<ListItem>();


           
           foreach (object type in Enum.GetValues(TypeEnem))
           {
               string str=type.ToString();
               if (str.Contains('X'))
               {

                   str = str.Replace("X", "/");
               }
               TypeList.Add(new ListItem(str, ((int)type).ToString()));
           }
           list.DataSource = TypeList;
           list.DataTextField = "text";
           list.DataValueField = "value";
           list.DataBind();
       }

       /// <summary>
       /// 讲枚举绑定在DropDownList上
       /// </summary>
       /// <param name="list">类型</param>
       /// <param name="text">第一项文字</param>
       /// <param name="TypeEnum">枚举(默认第一项为0)</param>
       public static void BindDropDownList_Replace(DropDownList list, Type TypeEnum)
       {
           List<ListItem> TypeList = new List<ListItem>();

           // TypeList.Add(new ListItem(text, value));
           //TypeList.Add(new ListItem { Text = "" + text + "", Value = "0" });


           foreach (object type in Enum.GetValues(TypeEnum))
           {
               string str=type.ToString();
               if (str.Contains('N') || str.Contains('D') || str.Contains('G')||str.Contains('M')||str.Contains('Q'))
               {
                   str=str.Replace("N", "$");
                   str=str.Replace("D", ",");
                   str=str.Replace("G", " - ");
                   str = str.Replace("M", ".");
                   str = str.Replace("Q", "");
               }
               TypeList.Add(new ListItem(str, ((int)type).ToString()));
           }
           list.DataSource = TypeList;
           list.DataTextField = "text";
           list.DataValueField = "value";
           list.DataBind();

       }

       /// <summary>
       /// 选中某一项
       /// </summary>
       /// <param name="radio">类型</param>
       /// <param name="value">值</param>
       public static void RadioButtonChecked(RadioButtonList radio,string value)
       {
           
           for (int i = 0; i < radio.Items.Count; i++)
           {
               if (radio.Items[i].Value != value)
               {
                   radio.Items[i].Selected = false;
                  // continue;
               }
               else
               {
                   radio.ClearSelection();
                   radio.Items[i].Selected = true;
                  
               }
           }
       }


       /// <summary>
       /// 选中某一项
       /// </summary>
       /// <param name="radio">类型</param>
       /// <param name="value">值</param>
       public static void DropDownListChecked(DropDownList drop, string value)
       {
           
           for (int i = 0; i <drop.Items.Count; i++)
           {
               if (drop.Items[i].Value != value)
               {
                   drop.Items[i].Selected = false;
                   //continue;
               }
               else
               {
                   drop.ClearSelection();
                   drop.Items[i].Selected = true;
                  
               }
           }
       }


       /// <summary>
       /// 选中某一项
       /// </summary>
       /// <param name="radio">类型</param>
       /// <param name="value">值</param>
       public static void DropListChecked(DropDownList drop, string text)
       {
          // drop.ClearSelection();
           for (int i = 0; i < drop.Items.Count; i++)
           {
               if (drop.Items[i].Text != text)
               {
                   drop.Items[i].Selected = false;
                   //continue;
               }
               else
               {
                   drop.ClearSelection();
                   drop.Items[i].Selected = true;

               }
           }
       }

       /// <summary>
       /// 绑定枚举在CheckBoxList上
       /// </summary>
       /// <param name="list"></param>
       /// <param name="TypeEnum"></param>
       public static void BindCheckBoxList(CheckBoxList list, Type TypeEnum)
       {
           List<ListItem> TypeList = new List<ListItem>();
           foreach (object type in Enum.GetValues(TypeEnum))
           {
               TypeList.Add(new ListItem(type.ToString(), ((int)type).ToString()));
           }
           list.DataSource = TypeList;
           list.DataTextField = "text";
           list.DataValueField = "value";
           list.DataBind();

       }

     

    }