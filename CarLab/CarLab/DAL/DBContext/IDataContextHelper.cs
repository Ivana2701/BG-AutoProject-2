using System;
using System.Collections.Generic;
using System.Text;

namespace CarLab.DAL.DBContext
{
   public interface IDataContextHelper
    {
        public string ConnetionString { get; }
        public string providerName { get; }

        public CarLabConnDB GetPPContextHelper(bool enableAutoSelect = true);
    }
}
