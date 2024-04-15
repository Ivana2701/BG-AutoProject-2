using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BGAuto.DAL.DBContext
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

        public BGAutoConnDB GetPPContextHelper(bool enableAutoSelect = true)
        {
       
            return (GetNewDataContext(ConnetionString, providerName, enableAutoSelect));
        }

        private static BGAutoConnDB GetNewDataContext(string ConnetionString,string providerName, bool enableAutoSelect)
        {
            BGAutoConnDB repository = new BGAutoConnDB(ConnetionString, providerName);
            repository.EnableAutoSelect = enableAutoSelect;
            //repository.ELHelperInstance = elHelperInstance;



            return (repository);
        }
    }
}
