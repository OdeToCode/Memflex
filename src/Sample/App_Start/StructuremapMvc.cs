using System.Web.Mvc;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LogMeIn.App_Start.StructuremapMvc), "Start")]

namespace LogMeIn.App_Start {
    public static class StructuremapMvc {
        public static void Start() {
            var container = IoC.Initialize();
            DependencyResolver.SetResolver(new SmDependencyResolver(container));
        }
    }
}