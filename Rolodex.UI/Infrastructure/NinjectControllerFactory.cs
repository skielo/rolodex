using Ninject;
using Rolodex.Domain.Abstract;
using Rolodex.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rolodex.UI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            this.AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, System.Type controllerType)
        {
            return controllerType == null ? null :
                (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IPeopleRepository>().To<PeopleRepository>();
        }
    }
}