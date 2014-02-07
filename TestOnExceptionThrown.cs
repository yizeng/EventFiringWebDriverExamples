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
			IWebDriver parentDriver = new FirefoxDriver();

			EventFiringWebDriver eventFiringWebDriver = new EventFiringWebDriver(parentDriver);
			eventFiringWebDriver.ExceptionThrown += new EventHandler<WebDriverExceptionEventArgs>(eventFiringWebDriver_TakeScreenshotOnException);

			driver = eventFiringWebDriver;
			driver.Navigate().GoToUrl("http://stackoverflow.com");
		}

		private void eventFiringWebDriver_TakeScreenshotOnException(object sender, WebDriverExceptionEventArgs e) {
			driver.TakeScreenshot().SaveAsFile("Exception.png", ImageFormat.Png);
		}

		[TestMethod]
		public void LoadStackOverflow() {
			Assert.IsTrue(driver.FindElement(By.CssSelector("#hlogo > a")).Displayed);
		}

		[TestMethod]
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
