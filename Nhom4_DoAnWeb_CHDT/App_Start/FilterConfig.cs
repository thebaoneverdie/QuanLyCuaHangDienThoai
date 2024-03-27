using System.Web;
using System.Web.Mvc;

namespace Nhom4_DoAnWeb_CHDT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
