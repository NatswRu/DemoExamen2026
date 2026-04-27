using DemoExamen2026.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace DemoExamen2026.Statics
{
    public static class CurrentSession
    {
        public static UserTable CurrentUser { get; set; }
    }
}
