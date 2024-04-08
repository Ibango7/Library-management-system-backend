using System.Threading.Tasks;
using LmsBackend.Models.TokenAuth;
using LmsBackend.Web.Controllers;
using Shouldly;
using Xunit;

namespace LmsBackend.Web.Tests.Controllers
{
    public class HomeController_Tests: LmsBackendWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}