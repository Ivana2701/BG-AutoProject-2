using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarLab.DAL.DBContext
{
    public class DataContextHelper: IDataContextHelper
    {
        private   readonly IConfiguration _configuration;


        public DataContextHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnetionString = _configuration.GetConnectionString("DBConnection");
            providerName = "System.Data.SqlClient";
        }
        public string ConnetionString { get; }
        public string providerName { get; }

        public CarLabConnDB GetPPContextHelper(bool enableAutoSelect = true)
        {
       
            return (GetNewDataContext(ConnetionString, providerName, enableAutoSelect));
        }

        private static CarLabConnDB GetNewDataContext(string ConnetionString,string providerName, bool enableAutoSelect)
        {
            CarLabConnDB repository = new CarLabConnDB(ConnetionString, providerName);
            repository.EnableAutoSelect = enableAutoSelect;
            //repository.ELHelperInstance = elHelperInstance;



            return (repository);
        }
    }
}
