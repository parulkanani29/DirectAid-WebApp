using System.Collections.Generic;
using System.Security.Permissions;
using SharedModels;

namespace AidWebApp.Models
{
    public class DashboardViewModel
    {
        public ulong Balance { get; set; }
        public List<ApplicationViewModel> Application { get; set; }
        
    }
}
