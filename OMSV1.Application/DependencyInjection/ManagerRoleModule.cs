using Autofac;
using MediatR;
using OMSV1.Application.Queries.Offices;
using OMSV1.Application.Queries.Governorates;
using OMSV1.Application.Queries.DamagedPassports;
using OMSV1.Application.Queries.DamagedDevices;
using OMSV1.Application.Handlers.DamagedDevices;
using OMSV1.Application.Handlers.DamagedPassports;
using OMSV1.Application.Handlers.Governorates;
using OMSV1.Application.Handlers.Offices;

namespace OMSV1.Application.DependencyInjection
{
    public class ManagerRoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register read-only handlers for Manager role
            builder.RegisterType<GetAllOfficesQueryHandler>()
                .As<IRequestHandler<GetAllOfficesQuery, IReadOnlyList<OMSV1.Domain.Entities.Offices.Office>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GetAllGovernoratesQueryHandler>()
                .As<IRequestHandler<GetAllGovernoratesQuery, IReadOnlyList<OMSV1.Domain.Entities.Governorates.Governorate>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GetAllDamagedPassportsQueryHandler>()
                .As<IRequestHandler<GetAllDamagedPassportsQuery, IReadOnlyList<OMSV1.Domain.Entities.DamagedPassport.DamagedPassport>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GetAllDamagedDevicesQueryHandler>()
                .As<IRequestHandler<GetAllDamagedDevicesQuery, IReadOnlyList<OMSV1.Domain.Entities.DamagedDevices.DamagedDevice>>>()
                .InstancePerLifetimeScope();
        }
    }
}
