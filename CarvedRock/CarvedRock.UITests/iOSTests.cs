using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Windows.Enums;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CarvedRock.UITests
{
    [TestClass]
    public class iOSTests
    {
        static TestContext ctx;

        [ClassInitialize]
        static public void Initialize(TestContext context)
        {
            ctx = context;
        }
        [TestMethod]
        public void ScrollToEndOfListUsingRemoteTouchScreenScroll()
        {
            var driver = StartApp();
            var touchScreen = new RemoteTouchScreen(driver);
            touchScreen.Scroll(0, -300);
            touchScreen.Scroll(0, -300);

            driver.CloseApp();

        }

        [TestMethod]
        public void GetUIDocument()
        {
            var driver = StartApp();

            var document = driver.PageSource;
            ctx.WriteLine(document);

        }

        [TestMethod]
        public void TapElementWeFind()
        {
            var driver = StartApp();

            var ListView = driver.FindElement(MobileBy.ClassName("ListView"));
            ListView.Click();

            driver.CloseApp();
        }
        [TestMethod]
        public void ScrollToEndOfListUsingPointerInputDevice()
        {
            var driver = StartApp();
            var ListView = driver.FindElement(MobileBy.ClassName("ListView"));

            // set start point
            SwipeUp(driver, ListView);

            Thread.Sleep(3000);

            SwipeUp(driver, ListView);


            Thread.Sleep(3000);

            driver.CloseApp();

        }

        [TestMethod]
        public void CheckMasterDetailAndBack()
        {

            IOSDriver<IOSElement> driver = StartApp();
            // tap on second item
            var el1 = driver.FindElementByAccessibilityId("Second item");
            el1.Click();
            
            var el2 = driver.FindElementByAccessibilityId("ItemText");
            var txt = el2.Text;
            Assert.IsTrue(txt == "Second item");

            // find root view controller link
            var elback = driver.FindElementByAccessibilityId("Root View Controller");
            elback.Click();

            var el3 = driver.FindElementByAccessibilityId("Fourth item");
            Assert.IsTrue(el3 != null);

            driver.CloseApp();

        }

        [TestMethod]
        public void AddNewItem()
        {
            IOSDriver<IOSElement> driver = StartApp();
            // tap on second item
            var el1 = driver.FindElementByAccessibilityId("Add");
            el1.Click();

            var elItemText = driver.FindElementByAccessibilityId("ItemText");
            elItemText.Clear();
            elItemText.SendKeys("This is a new Item");

            var elItemDetail = driver.FindElementByAccessibilityId("ItemDetailsText");
            elItemDetail.Clear();
            elItemDetail.SendKeys("These are the details");

            var elSave = driver.FindElementByAccessibilityId("Add");
            CreateScreenshot(driver);
            elSave.Click();

            var scrollableElement = driver.FindElementByAccessibilityId("maintable");

            Func<AppiumWebElement> FindElementAction = () =>
            {
                // find all text views
                // check if the text matches
                var element = driver.FindElementByAccessibilityId("This is a new Item");

                return element;
            };

            var elementFound = ScrollUntillItemFound(driver, scrollableElement, FindElementAction);


            Assert.IsTrue(elementFound != null);
            driver.CloseApp();

        }

        private AppiumWebElement ScrollUntillItemFound(IOSDriver<IOSElement> driver, AppiumWebElement relativeTo, Func<AppiumWebElement> FindElementAction)
        {
            var wait = new DefaultWait<IOSDriver<IOSElement>>(driver)
            {
                Timeout = TimeSpan.FromSeconds(60),
                PollingInterval = TimeSpan.FromMilliseconds(1000)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            AppiumWebElement elementfound = null;

            elementfound = wait.Until(d =>
            {
                SwipeUp(driver, relativeTo);

                return FindElementAction();
            });

            return elementfound;
        }

        
        private void CreateScreenshot(IOSDriver<IOSElement> driver)
        {
            var screenshot = driver.GetScreenshot();
            screenshot.SaveAsFile("startScreen.png", OpenQA.Selenium.ScreenshotImageFormat.Png);
            ctx.AddResultFile("startScreen.png");
        }

        private static void SwipeUp(IOSDriver<IOSElement> driver, AppiumWebElement element)
        {
            string script = "mobile: swipe";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("direction", "up");
            parameters.Add("element", element.Id);
            driver.ExecuteScript(script, parameters);
        }

        private static IOSDriver<IOSElement> StartApp()
        {
            System.Environment.SetEnvironmentVariable("DEVELOPER_DIR", @"/Applications/Xcode.app");

            var capabilities = new AppiumOptions();
            capabilities.AddAdditionalCapability(IOSMobileCapabilityType.BundleId, "com.fluentbytes.carvedrock");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformName, "ios");
            capabilities.AddAdditionalCapability(MobileCapabilityType.DeviceName, "iPhone 11");
            capabilities.AddAdditionalCapability(MobileCapabilityType.AutomationName, "XCUITest");
            capabilities.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "13.3");

            var driver = new IOSDriver<IOSElement>(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
            return driver;
        }

    }
}

