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
   public class LoanRepaymentCalculatorShould
   {

      [Test]
      [TestCase(200_000, 6.5, 30, 1264.14)]
      [TestCase(200_000, 10, 30, 1755.14)]
      [TestCase(500_000, 10, 30, 4387.86)]
      public void CalculatedcorrectMonthlyRepayment(decimal principal,
                                                     decimal interestRate,
                                                     int termInYears,
                                                     decimal expectedMonthlyPayment)
      {
         var sut = new LoanRepaymentCalculator();

         var monthlyPayment = sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal),
                                                            interestRate,
                                                            new LoanTerm(termInYears));

         Assert.That(monthlyPayment, Is.EqualTo(expectedMonthlyPayment));

      }

      [Test]
      [TestCase(200_000, 6.5, 30, ExpectedResult = 1264.14)]
      [TestCase(200_000, 10, 30, ExpectedResult = 1755.14)]
      [TestCase(500_000, 10, 30, ExpectedResult = 4387.86)]
      public decimal CalculatedcorrectMonthlyRepayment_SimplifiedTestCase(decimal principal,
                                                    decimal interestRate,
                                                    int termInYears)
      {
         var sut = new LoanRepaymentCalculator();

         return sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal),
                                                            interestRate,
                                                            new LoanTerm(termInYears));


      }

      // Test data in separate class
      [Test]
      [TestCaseSource(typeof(MonthlyRepaymentTestData), "TestCases")]
      public void CalculatedcorrectMonthlyRepayment_Centralized(decimal principal,
                                                    decimal interestRate,
                                                    int termInYears,
                                                    decimal expectedMonthlyPayment)
      {
         var sut = new LoanRepaymentCalculator();

         var monthlyPayment = sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal),
                                                            interestRate,
                                                            new LoanTerm(termInYears));

         Assert.That(monthlyPayment, Is.EqualTo(expectedMonthlyPayment));

      }

      [Test]
      [TestCaseSource(typeof(MonthlyRepaymentTestDataWithReturn), "TestCases")]
      public decimal CalculatedcorrectMonthlyRepayment_CentralizedWithReturn(decimal principal,
                                                   decimal interestRate,
                                                   int termInYears)
      {
         var sut = new LoanRepaymentCalculator();

         return sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal),
                                                            interestRate,
                                                            new LoanTerm(termInYears));


      }

      // Parameter level value attributes combines all different values automatically
      // useful to provide a wide range of values and make sure it doesn't throw an exception;
      [Test]
      public void CalculateCorrectMonthlyRepayment_Combinatorial(
           [Values (100_000, 200_000, 500_000)] decimal principal,
           [Values (6.5, 10, 20)] decimal interestRate,
           [Values (10, 20, 30)] int termInYears)
      {
         var sut = new LoanRepaymentCalculator();

         var monthlyPayment = sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal), interestRate, new LoanTerm(termInYears));

      }

      [Test]
      [Sequential]
      public void CalculateCorrectMonthlyRepayment_Sequential(
         [Values(200_000, 200_000, 500_000)] decimal principal,
         [Values(6.5, 10, 10)] decimal interestRate,
         [Values(30, 30, 30)] int termInYears, 
         [Values(1264.14, 1755.14, 4387.86)]decimal expectedMonthlyPayment)
      {
         var sut = new LoanRepaymentCalculator();

         var monthlyPayment = sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal), interestRate, new LoanTerm(termInYears));

         Assert.That(monthlyPayment, Is.EqualTo(expectedMonthlyPayment));
      }

      // Range Attribute 
      // create massive set of all range inputs
      [Test]
      public void CalculateCorrectMonthlyRepayment_Range(
         [Range(50_000, 1_000_000, 50_000)]decimal principal,
         [Range(0.5, 20.00, 0.5)]decimal interestRate,
         [Values(10, 20, 30)]int termInYears)
      {
         var sut = new LoanRepaymentCalculator();

         sut.CalculateMonthlyRepayment(new LoanAmount("USD", principal), interestRate, new LoanTerm(termInYears));
      }
   }
}
