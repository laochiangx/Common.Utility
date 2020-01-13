Microsoft ASP.NET Web API Tracing
=================================

This package allows the ASP.NET Web API framework to trace to System.Diagnostics.Trace.
 
To enable tracing in your application please add the following line of code
to your startup code.

If using C#, add the following line to WebApiConfig.cs (Global.asax.cs in an MVC 4 project):
    config.EnableSystemDiagnosticsTracing();

If using Visual Basic, add the following line to WebApiConfig.vb (Global.asax.vb in an MVC 4 project):
    config.EnableSystemDiagnosticsTracing()

where 'config' is the HttpConfiguration instance for your application.

For additional information on debugging and tracing in ASP.NET Web API, refer to:
    http://go.microsoft.com/fwlink/?LinkId=269874
