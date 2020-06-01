using System;
using Loans.Domain.Applications;
using NUnit.Framework;
using Moq;

namespace Loans.Tests
{
   public class LoanApplicationProcessorShould
   {
      [Test]
      public void DeclineLowSalary()
      {
         LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
         LoanAmount amount = new LoanAmount("USD", 200_000);
         var application = new LoanApplication(42,
                                               product,
                                               amount,
                                               "Sarah",
                                               25,
                                               "133 Pluralsight Drive, Draper, Utah",
                                               64_999);

         var mockIdentityVerifier = new Mock<IIdentityVerifier>();
         var mockCreditScorer = new Mock<ICreditScorer>();

         var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

         sut.Process(application);

         Assert.That(application.GetIsAccepted(), Is.False);
      }


      delegate void ValidateCallback(string applicantName,
                                        int applicantAge,
                                        string applicantAddress,
                                        ref IdentityVerificationStatus status);

      [Test]
      public void Accept()
      {
         LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
         LoanAmount amount = new LoanAmount("USD", 200_000);
         var application = new LoanApplication(42,
                                               product,
                                               amount,
                                               "Sarah",
                                               25,
                                               "133 Pluralsight Drive, Draper, Utah",
                                               65_000);

         var mockIdentityVerifier = new Mock<IIdentityVerifier>();
         // can use It method if you don't need to check against specific values
         //mockIdentityVerifier.Setup(x => x.Validate(It.IsAny<string>(),
         //                                      It.IsAny<int>(),
         //                                      It.IsAny<string>()))
         //                                     .Returns(true);

         mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                                             25,
                                             "133 Pluralsight Drive, Draper, Utah"))
                                            .Returns(true);


         var mockCreditScorer = new Mock<ICreditScorer>(); // { DefaultValue = DefaultValue.Mock }; mock will automatically set up hierarchy

         mockCreditScorer.SetupAllProperties();

         mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
        //mockCreditScorer.SetupProperty(x => x.Count);



         var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

         sut.Process(application);

         mockCreditScorer.VerifyGet(x => x.ScoreResult.ScoreValue.Score);
       //  mockCreditScorer.VerifySet(x => x.Count = 1);

         Assert.That(application.GetIsAccepted(), Is.True);
         Assert.That(mockCreditScorer.Object.Count, Is.EqualTo(1));
      }

      [Test]
      public void NullReturnExample()
      {
         var mock = new Mock<INullExample>();

         mock.Setup(x => x.SomeMethod()); 
            // .Returns<string>(null); //mock returns null object by default

         string mockReturnValue = mock.Object.SomeMethod();

         Assert.That(mockReturnValue, Is.Null);
      }

      [Test]
      public void InitializeIdentityVerifier()
      {
         LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
         LoanAmount amount = new LoanAmount("USD", 200_000);
         var application = new LoanApplication(42,
                                               product,
                                               amount,
                                               "Sarah",
                                               25,
                                               "133 Pluralsight Drive, Draper, Utah",
                                               65_000);

         var mockIdentityVerifier = new Mock<IIdentityVerifier>();

         mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                                             25,
                                             "133 Pluralsight Drive, Draper, Utah"))
                                            .Returns(true);


         var mockCreditScorer = new Mock<ICreditScorer>(); 
         mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
         var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

         sut.Process(application);

         mockIdentityVerifier.Verify(x => x.Initialize());

         mockIdentityVerifier.Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

         mockIdentityVerifier.VerifyNoOtherCalls();
      }

      [Test]
      public void CalculateScore()
      {
         LoanProduct product = new LoanProduct(99, "Loan", 5.25m);
         LoanAmount amount = new LoanAmount("USD", 200_000);
         var application = new LoanApplication(42,
                                               product,
                                               amount,
                                               "Sarah",
                                               25,
                                               "133 Pluralsight Drive, Draper, Utah",
                                               65_000);

         var mockIdentityVerifier = new Mock<IIdentityVerifier>();

         mockIdentityVerifier.Setup(x => x.Validate("Sarah",
                                             25,
                                             "133 Pluralsight Drive, Draper, Utah"))
                                            .Returns(true);


         var mockCreditScorer = new Mock<ICreditScorer>();
         mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);
         var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

         sut.Process(application);

         mockCreditScorer.Verify(x => x.CalculateScore("Sarah", "133 Pluralsight Drive, Draper, Utah"));
      }

   }

   public interface INullExample
   {
      string SomeMethod();
   }
}
