using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Utilities.Job
{
    public class TestJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //这里写任务逻辑
            new RunTask();
        }
    }

     class RunTask
     {
        
     }
}
