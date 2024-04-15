using System;
using System.Collections.Generic;
using System.Text;

namespace BGAuto.DAL.DBContext
{
   public interface IDataContextHelper
    {
        public string ConnetionString { get; }
        public string providerName { get; }

        public BGAutoConnDB GetPPContextHelper(bool enableAutoSelect = true);
    }
}
