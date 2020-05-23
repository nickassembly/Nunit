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
   public class ProductComparerShould
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

      [Test]
      public void NotReturnDuplicateComparisons()
      {
         var products = new List<LoanProduct>
          {
           new LoanProduct(1, "a", 1),
           new LoanProduct(2, "b", 2),
           new LoanProduct(3, "c", 3),
          };

         var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         Assert.That(comparisons, Is.Unique);

      }

      [Test]
      public void ReturnComparisonForFirstProduct()
      {
         var products = new List<LoanProduct>
          {
           new LoanProduct(1, "a", 1),
           new LoanProduct(2, "b", 2),
           new LoanProduct(3, "c", 3),
          };

         var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         // Need to know the expected monthly repayment value
         var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);

         Assert.That(comparisons, Does.Contain(expectedProduct));

      }

      [Test]
      public void ReturnComparisonForFirstProduct_WithPartialKnownExpectedValues()
      {
         var products = new List<LoanProduct>
          {
           new LoanProduct(1, "a", 1),
           new LoanProduct(2, "b", 2),
           new LoanProduct(3, "c", 3),
          };

         var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         // Don't care about expected monthly repayment, only that the product is there
         // These assertions are brittle, they require properties to be strings
         //Assert.That(comparisons, Has.Exactly(1)
         //                            .Property("ProductName").EqualTo("a")
         //                            .And
         //                            .Property("InterestRate").EqualTo(1)
         //                            .And
         //                            .Property("MonthlyRepayment").GreaterThan(0));

         // a more type safe version
         Assert.That(comparisons, Has.Exactly(1)
                                     .Matches<MonthlyRepaymentComparison>(item => item.ProductName == "a"
                                                                               && item.InterestRate == 1
                                                                               && item.MonthlyRepayment > 0));

      }




   }
}
