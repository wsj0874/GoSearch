using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoSearch
{
    public class Singleton<T> where T :new()
    {
        private static readonly T instance = new T();

        public static T Instance
        {
            get
            {
                return instance;
            }
        }
    }
}