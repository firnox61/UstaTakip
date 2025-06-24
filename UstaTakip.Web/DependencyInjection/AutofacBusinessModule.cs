using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using UstaTakip.Application.Interfaces.Security;
using UstaTakip.Application.Repositories;
using UstaTakip.Application.Services.Managers;
using UstaTakip.Core.Aspects.Validation;
using UstaTakip.Domain.Security;
using UstaTakip.Infrastructure.Aspects.Interceptors;
using UstaTakip.Infrastructure.Persistence.Repositories.EntityFramework;
using UstaTakip.Infrastructure.Security.Hashing;
using UstaTakip.Infrastructure.Security.Jwt;

namespace UstaTakip.Web.DependencyInjection
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           
          //  builder.RegisterType<AuthManager>().As<IAuthService>();
          //  builder.RegisterType<EfUserDal>().As<IUserDal>();
         //   builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

         //    builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();

         //     builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            builder.RegisterType<HashingService>().As<IHashingService>();

          //  builder.RegisterType<AuditLogCleanupJob>().AsSelf();

            var managerAssembly = typeof(CustomerManager).Assembly; // En net yol
              var coreAssembly = typeof(ValidationAspect).Assembly;
              var infrastructureAssembly = typeof(AspectInterceptorSelector).Assembly;

              builder.RegisterAssemblyTypes(managerAssembly, coreAssembly, infrastructureAssembly)
                  .AsImplementedInterfaces()
                  .EnableInterfaceInterceptors(new ProxyGenerationOptions
                  {
                      Selector = new AspectInterceptorSelector()
                  })
                  .InstancePerLifetimeScope();
            /*builder.RegisterType<IngredientManager>()
      .As<IIngredientService>()
      .EnableInterfaceInterceptors(new ProxyGenerationOptions
      {
          Selector = new AspectInterceptorSelector()
      })
      .InstancePerLifetimeScope();*/






        }
    }
}
