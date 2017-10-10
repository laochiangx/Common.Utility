using System.Reflection;
using System.Runtime.CompilerServices;

//
// 有关程序集的常规信息是通过下列
// 属性集控制的。更改这些属性值可修改与程序集
// 关联的信息。
//
[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

//
// 程序集的版本信息由下列 4 个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 您可以指定所有这些值，也可以使用“修订号”和“内部版本号”的默认值，方法是按
// 如下所示使用 '*':

[assembly: AssemblyVersion("1.0.*")]

//
// 要对程序集进行签名，必须指定要使用的密钥。有关程序集签名的更多信息，请参考
// Microsoft .NET Framework 文档。
//
// 使用下面的属性控制用于签名的密钥。
//
// 注意: 
//   (*) 如果未指定密钥，则程序集不会被签名。
//   (*) KeyName 是指已经安装在
//       计算机上的加密服务提供程序(CSP)中的密钥。KeyFile 是指包含
//       密钥的文件。
//   (*) 如果 KeyFile 和 KeyName 值都已指定，则
//       发生下面的处理: 
//       (1) 如果在 CSP 中可以找到 KeyName，则使用该密钥。
//       (2) 如果 KeyName 不存在而 KeyFile 存在，则 
//           KeyFile 中的密钥安装到 CSP 中并且使用该密钥。
//   (*) 要创建 KeyFile，可以使用 sn.exe(强名称)实用工具。
//        在指定 KeyFile 时，KeyFile 的位置应该
//        相对于“项目输出目录”。项目输出
//        目录的位置取决于您是在使用本地项目还是 Web 项目。
//        对于本地项目，项目输出目录定义为
//       <Project Directory>\obj\<Configuration>。例如，如果 KeyFile 位于该
//       项目目录中，应将 AssemblyKeyFile 
//       属性指定为 [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//        对于 Web 项目，项目输出目录定义为
//       %HOMEPATH%\VSWebCache\<Machine Name>\<Project Directory>\obj\<Configuration>。
//   (*) “延迟签名”是一个高级选项 - 有关它的更多信息，请参阅 Microsoft .NET Framework
//       文档。
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
