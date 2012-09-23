using System.Collections.Generic;
using System.Web.Mvc;
using StructureMap;

namespace LogMeIn
{
    public class SmFilterProvider : FilterAttributeFilterProvider
    {
        public SmFilterProvider(IContainer container)
        {
            _container = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            foreach (var filter in filters)
            {
                _container.BuildUp(filter.Instance);
            }

            return filters;
        }

        private IContainer _container;
    }
}