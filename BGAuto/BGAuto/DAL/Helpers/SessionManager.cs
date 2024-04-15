using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BGAuto.DAL.Services;
using BGAuto.Models.DbEntities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace BGAuto.DAL.Helpers
{
    public class SessionManager : ControllerBase, ISessionManager
    {
        private readonly IHttpContextAccessor _contx;
        private readonly IBasicDataServices _basicData;
        private readonly IConfiguration _configuration;
        public SessionManager(IHttpContextAccessor contx,  IBasicDataServices basicData, IConfiguration configuration)
        {
            this._contx = contx;
            this._basicData = basicData;
            this._configuration = configuration;
        }


        private const string KEY = "ARG@PLIS";
        private const string IV = "HUN@IDIS";


        //this method is getting login user info data from session
        public Users GetLoginUserFromSession()
        {

            Users user = new Users();
            try
            {
                var Users = _contx!=null && _contx.HttpContext!=null && _contx.HttpContext.Session!=null ? _contx.HttpContext.Session.GetString("User") : null;
                if (!String.IsNullOrEmpty(Users))
                {
                    user = JsonConvert.DeserializeObject<Users>(Users);
                    if (user == null)
                    {
                        //get user data from cookie and set user session
                        string SQN = "-1";
                        string UserID = "-1";
                        SQN = _contx.HttpContext.Request.Cookies["SQN"] == null ? "-1" : _contx.HttpContext.Request.Cookies["SQN"];
                        UserID = _contx.HttpContext.Request.Cookies["UserID"] == null ? "-1" : _contx.HttpContext.Request.Cookies["UserID"];

                        user = this._basicData.GetUserDataByUserID( Convert.ToInt32(UserID));
                        if (user != null)
                        {
                            if (user.UserID > 0)
                            {
                                SetUserDataInSession(user);
                            
                            }

                        }
                        else
                        {
                            user = null;
                        }

                    }
                }
                else
                {
                    //get user data from cookie and set user session
                    string SQN = "-1";
                    string UserID = "-1";
                    SQN = _contx.HttpContext!=null && _contx.HttpContext.Request.Cookies["SQN"] == null ? "-1" : _contx.HttpContext.Request.Cookies["SQN"];
                    UserID = _contx.HttpContext.Request.Cookies["UserID"] == null ? "-1" : _contx.HttpContext.Request.Cookies["UserID"];

                    user = this._basicData.GetUserDataByUserID(Convert.ToInt32(UserID));
                    if (user != null)
                    {
                        if (user.UserID > 0)
                        {
                            SetUserDataInSession(user);

                        }

                    }
                    else
                    {
                        user = null;
                    }

                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                user = null;
            }


            return user;
        }


        //--set admin user data in session
        public void SetUserDataInSession(Users model)
        {
            //set session
            var user = JsonConvert.SerializeObject(model);
            _contx.HttpContext.Session.SetString("User", user);
            _contx.HttpContext.Session.SetInt32("UserID", model.UserID);
            _contx.HttpContext.Session.SetString("EmailAddress", String.IsNullOrWhiteSpace(model.EmailAddress) ? "" : model.EmailAddress);

            //set cookies
            CookieOptions options = new CookieOptions();

            int CookiesTimeInDays = !String.IsNullOrEmpty(_configuration.GetSection("AppSetting").GetSection("CookiesExpiryDurationInDays").Value) ? Convert.ToInt32(_configuration.GetSection("AppSetting").GetSection("CookiesExpiryDurationInDays").Value) : 1;

            options.Expires = DateTime.Now.AddDays(CookiesTimeInDays);

            this._contx.HttpContext.Response.Cookies.Append("UserID", Convert.ToString(model.UserID), options);
            this._contx.HttpContext.Response.Cookies.Append("EmailAddress", String.IsNullOrWhiteSpace(model.EmailAddress) ? "" : model.EmailAddress, options);
           
        }

        //--set customer user data in session
        public void SetCustomerUserDataInSession(Users model)
        {
            //set session
            var user = JsonConvert.SerializeObject(model);
            _contx.HttpContext.Session.SetString("CustomerUser", user);
            _contx.HttpContext.Session.SetInt32("CustomerUserID", model.UserID);
            _contx.HttpContext.Session.SetString("CustomerEmailAddress", String.IsNullOrWhiteSpace(model.EmailAddress) ? "" : model.EmailAddress);

            //set cookies
            CookieOptions options = new CookieOptions();

            int CookiesTimeInDays = !String.IsNullOrEmpty(_configuration.GetSection("AppSetting").GetSection("CookiesExpiryDurationInDays").Value) ? Convert.ToInt32(_configuration.GetSection("AppSetting").GetSection("CookiesExpiryDurationInDays").Value) : 1;

            options.Expires = DateTime.Now.AddDays(CookiesTimeInDays);

            this._contx.HttpContext.Response.Cookies.Append("CustomerUserID", Convert.ToString(model.UserID), options);
            this._contx.HttpContext.Response.Cookies.Append("CustomerEmailAddress", String.IsNullOrWhiteSpace(model.EmailAddress) ? "" : model.EmailAddress, options);

        }





        public string Encrypt(string text)
        {
            if (text.Trim().Length == 0)
                return string.Empty;
            byte[] bKey, bIV, bInput;

            bKey = System.Text.Encoding.UTF8.GetBytes(KEY);
            bIV = System.Text.Encoding.UTF8.GetBytes(IV);
            bInput = System.Text.Encoding.UTF8.GetBytes(text);

            MemoryStream memStream = new MemoryStream();

            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(memStream, des.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write);

            encStream.Write(bInput, 0, bInput.Length);
            encStream.FlushFinalBlock();
            string st = Convert.ToBase64String(memStream.ToArray());

            return (st);
        }

        public string Decrypt(string encText)
        {
            if (encText.Trim().Length == 0)
                return string.Empty;
            byte[] bKey, bIV, bInput;

            bKey = System.Text.Encoding.UTF8.GetBytes(KEY);
            bIV = System.Text.Encoding.UTF8.GetBytes(IV);
            bInput = Convert.FromBase64String(encText);

            MemoryStream memStream = new MemoryStream();

            DES des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(memStream, des.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write);

            encStream.Write(bInput, 0, bInput.Length);
            encStream.FlushFinalBlock();

            return (System.Text.Encoding.UTF8.GetString(memStream.ToArray()));
        }

    }


    public interface ISessionManager
    {
        public Users GetLoginUserFromSession();
        public void SetUserDataInSession(Users model);
        public string Encrypt(string text);
        public string Decrypt(string encText);
        public void SetCustomerUserDataInSession(Users model);



    }
}
