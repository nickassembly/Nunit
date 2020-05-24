using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Loans.Test
{
   public class MonthlyRepaymentTestDataWithReturn
   {
      public static IEnumerable TestCases
      {
         get
         {
            yield return new TestCaseData(200_000m, 6.5m, 30).Returns(1264.14);
            yield return new TestCaseData(200_000m, 10m, 30).Returns(1755.14);
            yield return new TestCaseData(500_000m, 10m, 30).Returns(4387.86);
         }
      }

   }
}
