using System.Reflection;
using Autofac;
using MediatR;
using OMSV1.Application.Handlers.Offices;
using OMSV1.Application.Queries.Offices;

namespace OMSV1.Application.DependencyInjection
{
    public class SupervisorRoleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Supervisor-specific handlers
            builder.RegisterType<GetAllOfficesQueryHandler>()
                .As<IRequestHandler<GetAllOfficesQuery, IReadOnlyList<Domain.Entities.Offices.Office>>>()
                .InstancePerLifetimeScope();
        }
    }
}
