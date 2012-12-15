﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;
using Unilib.Frontend.Injection;
using log4net;

namespace Unilib.Frontend
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
           /* Bus = Configure.With()
.Log4Net()
.DefaultBuilder()
.RijndaelEncryptionService()
.XmlSerializer("http://acme.com")
.InMemoryFaultManagement()
.MsmqTransport()
.UnicastBus()
.CreateBus()
.Start(() => Configure.Instance
.ForInstallationOn<Windows>().Install());*/
            // NServiceBus configuration
            Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .ForMvc()
                .RunCustomAction(() => Configure.Instance.Configurer.ConfigureComponent(() => LogManager.GetLogger("Loger"), DependencyLifecycle.SingleInstance))
                .XmlSerializer()
                .MsmqTransport()
                .PurgeOnStartup(true)
                .UnicastBus()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }
    }
}