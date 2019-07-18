using System;
using System.Net.Http;
using Lambda3.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SampleApp;

namespace IntegrationTests
{
    public abstract class BaseIntegrationTest
    {
        protected HttpClient client;
        private IServiceScope scope;
        protected IServiceProvider serviceProvider;

        public static WebApplicationFactory<Startup> WebAppFactory { get; set; }

        [OneTimeSetUp]
        public void BaseIntegrationTestOneTimeSetUp()
        {
            client = WebAppFactory.CreateDefaultClient();
            scope = WebAppFactory.Host.Services.CreateScope();
            serviceProvider = scope.ServiceProvider;
        }

        [OneTimeTearDown]
        public void BaseIntegrationTestOneTimeTearDown()
        {
            scope.Dispose();
        }
    }
}