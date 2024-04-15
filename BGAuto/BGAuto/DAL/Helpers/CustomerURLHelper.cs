using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BGAuto.DAL.Helpers
{
    public class CustomerURLHelper : ICustomerURLHelper
    {
        private readonly IHttpContextAccessor _contx;
        private readonly IConfiguration _configuration;


        public CustomerURLHelper(IHttpContextAccessor contx, IConfiguration configuration)
        {
            this._contx = contx;
            this._configuration = configuration;

        }

        public string GetSiteBaseURL()
        {
            string BaseURL = _configuration.GetSection("AppSetting").GetSection("WebsiteBaseURL").Value;
            return BaseURL;
        }

        public string GetAdminHomePageURL()
        {
            string BaseURL = _configuration.GetSection("AppSetting").GetSection("AdminHomePageURL").Value;
            return BaseURL;
        }

    }


    public interface ICustomerURLHelper
    {
       
        public string GetSiteBaseURL();
        public string GetAdminHomePageURL();



    }
}
