using AutoMapper;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ASI.Basecode.WebApp
{
    // AutoMapper configuration
    internal partial class StartupConfigurer
    {
        /// <summary>
        /// Configure auto mapper
        /// </summary>
        private void ConfigureAutoMapper()
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileConfiguration());
            });

            this._services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());
        }

        private class AutoMapperProfileConfiguration : Profile
        {
            public AutoMapperProfileConfiguration()
            {
                CreateMap<UserViewModel, User>()
                    .ForMember(destination => destination.Id, source => source.Ignore())
                    .ForMember(destination => destination.Password, source => source.Ignore());
                CreateMap<UserEditViewModel, User>()
                    .ForMember(destination => destination.Id, source => source.Ignore())
                    .ForMember(destination => destination.Password, source => source.Ignore());

                CreateMap<BookMasterViewModel, BookMaster>()
                    .ForMember(destination => destination.BookId, source => source.Ignore());
                   // .ForMember(destination => destination.BookImage, source => source.MapFrom(src => src.BookImageFile));

                CreateMap<BookMasterEditViewModel, BookMaster>()
                    .ForMember(destination => destination.BookId, source => source.Ignore());
            }
        }
    }
}
