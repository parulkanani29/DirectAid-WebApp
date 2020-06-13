using AidWebApp.Models;
using AutoMapper;
using SharedModels;

namespace AidWebApp.Extensions
{
    public static class AutoMapperStartupTask
    {
        public static void Execute()
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.ValidateInlineMaps = false;

                    cfg.CreateMap<ApplicationViewModel, Application>()
                        .ReverseMap();

                    cfg.CreateMap<UserViewModel, User>()
                    .ReverseMap();
                });
        }
    }
}
