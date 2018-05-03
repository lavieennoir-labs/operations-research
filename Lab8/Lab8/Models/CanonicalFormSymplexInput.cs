using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Models
{
	public class CanonicalFormSymplexInput
	{
            public double[,] Matrix { get; set; }
            public double[] FreeMembers { get; set; }
    }
}