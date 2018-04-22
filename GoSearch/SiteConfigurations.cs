using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GoSearch
{
    public class SiteConfigurations
    {
        public SiteConfigurations()
        {
            {
                var properyInformationCollection = this.GetType().GetProperties();
                foreach (var propertyInformation in properyInformationCollection)
                {
                    if (propertyInformation.CanWrite)
                    {
                        //从APPSetting中得到数据
                        var configuration = ConfigurationManager.AppSettings[propertyInformation.Name];
                        if (configuration != null)
                        {
                            if (propertyInformation.PropertyType == typeof(string))
                            {
                                propertyInformation.SetValue(this, configuration);
                            }
                            else
                            {
                                propertyInformation.SetValue(this, Convert.ChangeType(configuration, propertyInformation.PropertyType));
                            }
                        }
                    }
                }
            }
        }
        public string RemoteAuthenticationServerUrlTemplate { get; private set; }
        public string ApplicationId { get; private set; }
        public string ApplicationSecret { get; private set; }

    }
}