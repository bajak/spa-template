using System.Web.Http.Dependencies;
using Ninject;

namespace spa.web
{
    public class DependencyResolver : DependencyScope, IDependencyResolver
    {
        private readonly IKernel kernel;

        public DependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyScope(kernel.BeginBlock());
        }
    }
}