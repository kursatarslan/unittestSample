using System;
using System.Threading.Tasks;
using AcceptanceTests.Pages;
using IntegrationTests;
using Lambda3.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using SampleApp;

namespace AcceptanceTests
{
    [SetUpFixture]
    public class Setup
    {
        private FrontendServer frontendServer;
        private WebApplicationFactory<Startup> webAppFactory;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            StartApiServer();
            StartFrontend();
            DriverManager.Start();
            await webAppFactory.MigrateDbAndSeedAsync();
            BaseAcceptanceTest.WebAppFactory = webAppFactory;
            Page.BaseUrl = frontendServer.BaseUrl;
            NavigateHome();
        }

        private void StartApiServer()
        {
            webAppFactory = new WebApplicationFactory<Startup>(5000).EnsureServerStarted();
        }

        private void StartFrontend()
        {
            frontendServer = new FrontendServer(ProjectFinder.FindProjectDir("src/FrontEnd"));
            frontendServer.StartFrontEnd();
        }

        private void NavigateHome()
        {
            try
            {
                new HomePage().Navigate();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not navigate home during test setup.", ex);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            RunAndSwallowException(() => DriverManager.Stop());
            RunAndSwallowException(() => frontendServer?.Dispose());
            RunAndSwallowException(() => webAppFactory?.Dispose());
        }

        private static void RunAndSwallowException(Action a)
        {
            try
            {
                a();
            }
            catch
            {
            }
        }
    }
}