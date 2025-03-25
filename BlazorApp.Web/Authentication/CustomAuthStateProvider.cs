using BlazorApp.Model.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace BlazorApp.Web.Authentication
{
    public class CustomAuthStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var sessionModel = (await localStorage.GetAsync<LoginResponseModel>("sessionState")).Value;
                var identity = sessionModel == null ? new ClaimsIdentity() : GetClaimsIdentity(sessionModel.Token);
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch(Exception ex)
            {
                await MarkUserAsLoggedOut();
                var identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
        }

        public async Task MarkUserAsAuthenticated(LoginResponseModel model)
        {
            await localStorage.SetAsync("sessionState", model);
            var identity = GetClaimsIdentity(model.Token);
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            return new ClaimsIdentity(claims, "jwt");
        }

        public async Task MarkUserAsLoggedOut()
        {
            await localStorage.DeleteAsync("sessionState");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
