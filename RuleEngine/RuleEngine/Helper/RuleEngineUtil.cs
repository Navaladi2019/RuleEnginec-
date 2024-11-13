﻿using RuleEngine.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine
{
    public static class RuleEngineUtil
    {
        public static RuleEngineStatus GetEngineStatus(this ExpandoObject ctx)
        {
            _ = (ctx as IDictionary<string, object>).TryGetValue("Status", out object status);
            return (RuleEngineStatus)status!;
        }

        public static void SetEngineStatus (this ExpandoObject ctx, RuleEngineStatus status)
        {
            (ctx as IDictionary<string, object>)["Status"] = status;
        }
        public static void SetRuleEngineErrMessage(this ExpandoObject ctx, string msg)
        {
            if (ctx.FirstOrDefault(x => x.Key == "ErrorMessage").Value == null)
            {
                (ctx as IDictionary<string, object>)["Status"] = new List<string>();
            }
            (ctx.First(x => x.Key == "ErrorMessage").Value as List<string>)!.Add(msg);
        }

        public static List<string> GetRuleEngineErrMessages(this ExpandoObject ctx)
        {
            return ctx.FirstOrDefault(x => x.Key == "ErrorMessage").Value as List<string> ?? [];
        }

       
    }
}