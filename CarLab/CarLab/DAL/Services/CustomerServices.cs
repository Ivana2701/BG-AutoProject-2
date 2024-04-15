using CarLab.DAL.DBContext;
using CarLab.Models.DbEntities;
using PetaPoco;
using System.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CarLab.DAL.Services
{
    public class CustomerServices : ICustomerServices
    {

        private readonly IConfiguration _configuration;
        private readonly IDatabase _db;
        private readonly IDataContextHelper _contextHelper;


        //--Constructor of the class
        public CustomerServices(IConfiguration configuration, IDatabase db, IDataContextHelper contextHelper)
        {
            _configuration = configuration;
            _db = db;
            _contextHelper = contextHelper;

        }



        public Users CustomerLogin(string EmailAddress, string Password)
        {
            Users result = new Users();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    repo.EnableAutoSelect = false;
                    result = repo.Fetch<Users>(@";EXEC [dbo].[SP_GetCustomerUserLogin] @EmailAddress , @Password",
                        new { EmailAddress = EmailAddress, Password = Password }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }


        public List<Products> GetCustomerOrderBookingList(int CustomerID)
        {

            List<Products> result = new List<Products>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Products>(@";EXEC [dbo].[SP_GetCustomerOrderBookingList] @CustomerID",
                        new { CustomerID = CustomerID}).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }



        public Users RegisterCustomerUser(string FullName, string EmailAddress, string Phone, string Password)
        {
            Users result = new Users();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    repo.EnableAutoSelect = false;
                    result = repo.Fetch<Users>(@";EXEC [dbo].[SP_CreateNewCustomer] @FullName , @EmailAddress , @Phone  , @Password ",
                        new { FullName = FullName, EmailAddress= EmailAddress, Phone= Phone, Password = Password }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }


    }

    public interface ICustomerServices
    {
        public Users CustomerLogin(string EmailAddress, string Password);

        public List<Products> GetCustomerOrderBookingList(int CustomerID);
        public Users RegisterCustomerUser(string FullName, string EmailAddress, string Phone, string Password);
    }
}
