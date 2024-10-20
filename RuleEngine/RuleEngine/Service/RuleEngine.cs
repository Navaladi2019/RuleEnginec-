﻿using RuleEngine.Helper;
using RuleEngine.Models;
using System.Dynamic;

namespace RuleEngine.Service
{
    public class RuleEngine
    {
        public RuleEngine(dynamic Inputparam, ITGRuleSet RulesetParam)
        {
            RuleSet = RulesetParam;
            Ctx = Utils.ConvertToExpando(Inputparam);
        }
        public readonly ITGRuleSet RuleSet;
        /// <summary>
        /// Global Context, it can be used to set object in rules and propogate from Rule A to B and so on.
        /// </summary>
        public required ExpandoObject Ctx { get; set; } = [];


        public async Task ExecuteAsync()
        {


        }
    }
}