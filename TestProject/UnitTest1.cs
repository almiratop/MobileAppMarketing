using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using TEST;
using Xamarin.Forms;

namespace TestProject
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void datab()
		{
			bool expected = true;
			Xamarin.Forms.Forms.Init();
			var af = new MainPage();
			bool actual = af.db();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Autoriz()
		{
			bool expected = true;
			Xamarin.Forms.Forms.Init();
			var af = new MainPage();
			bool actual = af.auth("almira", "123");
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Registr()
		{
			bool expected = true;
			Xamarin.Forms.Forms.Init();
			var af = new Registration();
			bool actual = af.Reg("георгий", "89050005432", "anon@mail.ru", "anon", "123");
			Assert.AreEqual(expected, actual);
		}
	}
}
