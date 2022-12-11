using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ValourSharp
{
    public class CommandInfo
    {
        public CommandInfo(MethodInfo method)
        {
            Method = method;
        }

        public MethodInfo Method { get; set; }

        private ParameterInfo[]? parameters = null;
        public ParameterInfo[] Parameters
        {
            get
            {
                if (parameters is not null) return parameters;

                parameters = Method.GetParameters();
                return parameters;
            }
        }
        private bool? allowBots = null;
        public bool AllowBots
        {
            get
            {
                if (allowBots is not null) return (bool)allowBots;

                allowBots = Method.GetCustomAttribute(typeof(AllowBots), false) is not null;
                return (bool)allowBots;
            }
        }

        private bool? allowSelf = null;
        public bool AllowSelf
        {
            get
            {
                if (allowSelf is not null) return (bool)allowSelf;

                allowSelf = Method.GetCustomAttribute(typeof(AllowSelf), false) is not null;
                return (bool)allowSelf;
            }
        }
    }
}
