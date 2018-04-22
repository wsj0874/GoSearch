using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoSearch.Service
{
    public class Common
    {
        public class UserStatus
        {
            public string Normal = "1";
            public string SessionOverdue = "-1";
        }
        public class TimeConversion
        {
            public int BeiJing = 8;
        }
        public class OperationList
        {
            public int GetPersonList = 1;
            public int GetPersonInfo = 2;
            public int GetPersonDetail = 3;
        }
    }
}