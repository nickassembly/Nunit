﻿using Loans.Domain.Applications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loans.Test
{
   [TestFixture]
   public class MonthlyReplaymentComparisonShould
   {
      [Test]
      public void ReturnCorrectNumberOfComparisons()
      {
         var products = new List<LoanProduct>
         {
            new LoanProduct(1, "a", 1),
            new LoanProduct(2, "b", 2),
            new LoanProduct(3, "c", 3),
         };

         var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         Assert.That(comparisons, Has.Exactly(3).Items);
      }



   }

}
