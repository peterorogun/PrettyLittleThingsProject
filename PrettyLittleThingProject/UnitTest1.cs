using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace PrettyLittleThingProject
{
    public class Tests
    {
        IWebDriver driver;
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.prettylittlething.com/";
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60);

        }

        [Test]
        public void Test1()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60))
            {
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until<IWebElement>(x => x.FindElement(By.XPath("//a[@class='cookie-preference__accept button'][.='Accept All']"))).Click();

            Thread.Sleep(1000);
            Actions action = new Actions(driver);
            action.MoveToElement(driver.FindElement(By.XPath("//*[@id='shoes']/a/span"))).Perform();
            var product = driver.FindElement(By.XPath("//ul[@class='level1']//li[.='All Boots']"));
            action.Click(product).Perform();
            var productName = driver.FindElement(By.XPath("//*[@class='product-title'][contains(text(), 'Behati Black Faux Croc Ankle Boot')]"));
            var ExpectedProductName = productName.Text;

            Thread.Sleep(3000);
            wait.Until<IWebElement>(x => x.FindElement(By.XPath
                ("//*[@class='product-title'][contains(text(), 'Behati Black Faux Croc Ankle Boot')]"))).Click();
            driver.FindElement(By.XPath("//div[@id='attribute128']/div[1]")).Click();
            driver.FindElement(By.Id("add-to-bag")).Click();
            driver.FindElement(By.Id("shopping-bag-text")).Click();
            var ActualProductName = driver.FindElement(By.XPath("(//p[@class='product-name'])[2]")).Text;
            Assert.AreEqual(ExpectedProductName, ActualProductName);
            var SubTotal = driver.FindElement(By.XPath("//p[@class='totals']/span")).Text;
            driver.FindElement(By.XPath("(//span[@class='track-cart-proceed analytics-tracking'])[1]")).Click();
            wait.Until<IWebElement>(x => x.FindElement(By.XPath("(//span[@data-event-action='Proceed To Checkout']/parent::button)[2]"))).Click();

            driver.FindElement(By.Id("customer-email")).Click();
            driver.FindElement(By.Id("customer-email")).SendKeys("peterorogun8@gmail.com");
            driver.FindElement(By.XPath("(//button[@class='btn c-button c-button--large '])[1]")).Click();

            
            var Subtotal2 = driver.FindElement(By.XPath("(//li[@class='bag__totals-item'])[2]//span[2]")).Text;
            Assert.AreEqual(SubTotal, Subtotal2);


            Thread.Sleep(1000);

        }

        [TearDown]
        public void quitbrowser()
        {
            driver.Quit();
        }
    }
}