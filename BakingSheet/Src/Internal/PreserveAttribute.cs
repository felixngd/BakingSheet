﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cathei.BakingSheet.Internal
{
    // Prevent Unity's code stripper
    [AttributeUsage(AttributeTargets.All)]
    public class PreserveAttribute : Attribute
    {
    }
}