using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestableCode
{
   public class Calculator
   {
      public decimal GetTotal(decimal parts, decimal service, decimal discount)
      {
         return parts + service - discount;
      }
   }
}
