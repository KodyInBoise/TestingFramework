﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Interfaces;

namespace TestingFramework.Models
{
    public class CategoryTestModel : ITest
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string Description { get; set; }
        public string ExpectedResult { get; set; }
        public double Value { get; set; }
        public int Order { get; set; }
    }
}
