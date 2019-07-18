using System;
using System.Net.Http;
using AcceptanceTests.Pages;
using Lambda3.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SampleApp;

namespace AcceptanceTests
{
    public class BaseAcceptanceTest
    {
        public static WebApplicationFactory<Startup> WebAppFactory { get; set; }
    }

    public abstract class BaseAcceptanceTest<TPage>
        where TPage : Page, new()
    {
        protected HttpClient client;
        private IServiceScope scope;
        protected IServiceProvider serviceProvider;

        public TPage Page { get; protected set; } = new TPage();

        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            client = BaseAcceptanceTest.WebAppFactory.CreateClient();
            scope = BaseAcceptanceTest.WebAppFactory.Host.Services.CreateScope();
            serviceProvider = scope.ServiceProvider;
        }

        [OneTimeTearDown]
        public void BaseOneTimeTearDown()
        {
            scope?.Dispose();
            client?.Dispose();
        }
    }
}