 


using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.Utility
{
    public static class Log
    {
        public static void Debug(object message)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Debug(message);
        }

        public static void Debug(object message, Exception ex)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Debug(message, ex);
        }

        public static void Error(object message)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Error(message, exception);
        }

      

        public static void Info(object message)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Info(message);
        }

        public static void Info(object message, Exception ex)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Info(message, ex);
        }

        public static void Warn(object message)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Warn(message);
        }

        public static void Warn(object message, Exception ex)
        {
            LogManager.GetLogger(Common.Utility.Log.GetCurrentMethodFullName()).Warn(message, ex);
        }

        private static string GetCurrentMethodFullName()
        {
            StackFrame frame;
            string str;
            string str1;
            bool flag;
            try
            {
                int num = 2;
                StackTrace stackTrace = new StackTrace();
                int length = stackTrace.GetFrames().Length;
                do
                {
                    int num1 = num;
                    num = num1 + 1;
                    frame = stackTrace.GetFrame(num1);
                    str = frame.GetMethod().DeclaringType.ToString();
                    flag = (!str.EndsWith("Exception") ? false : num < length);
                }
                while (flag);
                string name = frame.GetMethod().Name;
                str1 = string.Concat(str, ".", name);
            }
            catch
            {
                str1 = null;
            }
            return str1;
        }
    }
}
