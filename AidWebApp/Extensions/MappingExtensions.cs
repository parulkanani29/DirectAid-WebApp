using System.Collections.Generic;
using AidWebApp.Models;
using AutoMapper;
using SharedModels;

namespace AidWebApp.Extensions
{
    public static class MappingExtensions
    {
        public static List<ApplicationViewModel> ToListModel(this List<Application> sharedModel)
        {
            return Mapper.Map<List<Application>, List<ApplicationViewModel>>(sharedModel);
        }

        public static UserViewModel ToModel(this User sharedModel)
        {
            return Mapper.Map<User, UserViewModel>(sharedModel);
        }

    }
}
