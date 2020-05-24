using Loans.Domain.Applications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loans.Test
{
   [TestFixture]
   [ProductComparison] // custom attribute
   public class ProductComparerShould
   {

      private List<LoanProduct> products;
      private ProductComparer sut;

      [OneTimeSetUp]
      public void OneTimeSetUp()
      {
         // assumes list is not modified by any test. It is only executed once
         products = new List<LoanProduct>
          {
           new LoanProduct(1, "a", 1),
           new LoanProduct(2, "b", 2),
           new LoanProduct(3, "c", 3),
          };
      }

      [SetUp]
      public void Setup()
      {
          sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);
      }

      [OneTimeTearDown]
      public void OneTimeTearDown()
      {
         // can use to dispose of things
      }

      [TearDown]
      public void TearDown()
      {
         // Runs after each test executes

      }

      [Test]
      public void ReturnCorrectNumberOfComparisons()
      {
         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         Assert.That(comparisons, Has.Exactly(3).Items);

      }

      [Test]
      public void NotReturnDuplicateComparisons()
      {

         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         Assert.That(comparisons, Is.Unique);

      }

      [Test]
      public void ReturnComparisonForFirstProduct()
      {
         List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

         // Need to know the expected monthly repayment value
         var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);

         Assert.That(comparisons, Does.Contain(expectedProduct));

      }

      [Test]
      public void ReturnComparisonForFirstProduct_WithPartialKnownExpectedValues()
      {
    
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

         // custom constraint
         Assert.That(comparisons, Has.Exactly(1)
                                     .Matches(new MonthlyRepaymentGreaterThanZeroConstraint("a", 1)));

      }




   }
}
