using OpenQA.Selenium;

namespace AcceptanceTests.Pages
{
    public class HomePage : Page
    {
        public override string Path => "/";

        public bool HasTitle()
        {
            return Driver.FindElement(By.Id("title")).Displayed;
        }
    }
}