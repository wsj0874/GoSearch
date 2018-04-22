using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GoSearch
{
    public class CheckSession
    {
        private const int UserIdPositionInResponse = 0;
        private const int TimePositionInResponse = 1;
        private const int ZeroOfASCII = 48;
        private const int NineOfASCII = 57;
        private const double TimeDif = 8654000;
        private readonly SiteConfigurations siteConfigurations;

        public CheckSession() : this(Singleton<SiteConfigurations>.Instance)
        {
        }
        public CheckSession(SiteConfigurations siteConfigurations)
        {
            this.siteConfigurations = siteConfigurations;
        }
       

        public int GetUserId(string session)
        {
            int? Userid = null;
            try
            {
                string[] sArray = session.Split(';');
                Regex r = new Regex("\\d+\\.?\\d*");
                MatchCollection mc = r.Matches(sArray[UserIdPositionInResponse]);
                string result = string.Empty;
                for (int i = 0; i < mc.Count; i++)
                {
                    result += mc[i];
                }
                Userid = Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
            return Userid.Value;
        }

     
    }
}