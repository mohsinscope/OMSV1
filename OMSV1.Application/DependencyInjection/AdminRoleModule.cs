using System.Reflection;
using Autofac;
using MediatR;
using OMSV1.Application.Commands.Office;
using OMSV1.Application.Handlers.Offices;
using OMSV1.Application.Queries.Offices;
using OMSV1.Domain.Entities.Offices;

namespace OMSV1.Application.DependencyInjection
{
    public class AdminRoleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all handlers for Admin
            builder.RegisterType<AddOfficeCommandHandler>().As<IRequestHandler<AddOfficeCommand, int>>().InstancePerLifetimeScope();
            builder.RegisterType<GetAllOfficesQueryHandler>().As<IRequestHandler<GetAllOfficesQuery, IReadOnlyList<Office>>>().InstancePerLifetimeScope();

            // Add other Admin-specific handlers here
        }
    }
}
