using System;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;

namespace EventFiringWebDriverExamples {

	[TestClass]
	public class TestOnExceptionThrown {

		private IWebDriver driver;

		[TestInitialize]
		public void Initialize() {
			var firingDriver = new EventFiringWebDriver(new FirefoxDriver());
			firingDriver.ExceptionThrown += firingDriver_TakeScreenshotOnException;

			driver = firingDriver;
			driver.Navigate().GoToUrl("http://stackoverflow.com");
		}

		private void firingDriver_TakeScreenshotOnException(object sender, WebDriverExceptionEventArgs e) {
			string timestamp = DateTime.Now.ToString("yyyy-MM-dd-hhmm-ss");
			driver.TakeScreenshot().SaveAsFile("Exception-" + timestamp + ".png", ImageFormat.Png);
		}

		[TestMethod]
		public void LoadStackOverflow() {
			Assert.IsTrue(driver.FindElement(By.CssSelector("#hlogo > a")).Displayed);
		}

		[TestMethod]
		[ExpectedException(typeof(NoSuchElementException))]
		public void LoadStackOverflowWithException() {
			// Wrong locator, expect NoSuchElementException
			Assert.IsTrue(driver.FindElement(By.CssSelector("#hlogo > a > a > a")).Displayed);
		}

		[TestCleanup]
		public void Cleanup() {
			driver.Quit();
		}
	}
}
