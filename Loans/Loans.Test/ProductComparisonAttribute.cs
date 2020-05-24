using NUnit.Framework;
using System;

namespace Loans.Test
{

   [TestFixture]
   [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
   public class ProductComparisonAttribute : CategoryAttribute
   {

   }
}
