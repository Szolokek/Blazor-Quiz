using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kviz.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private readonly ILocalStorageService localStorageService;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await localStorageService.GetItemAsStringAsync("token");
                if (string.IsNullOrEmpty(token))
                {
                    return await Task.FromResult(new AuthenticationState(anonymous));
                }

                var id = await GetClaimsFromJwt(token);
                return await Task.FromResult(new AuthenticationState(SetClaimsPrincipal(id)));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthenticationState(string token)
        {
            ClaimsPrincipal claimsPrincipal;
            if (!string.IsNullOrEmpty(token))
            {
                await localStorageService.SetItemAsStringAsync("token", token);
                var id = await GetClaimsFromJwt(token);
                claimsPrincipal = SetClaimsPrincipal(id);
            }
            else
            {
                claimsPrincipal = anonymous;
                await localStorageService.RemoveItemAsync("token");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimsPrincipal(int id)
        {
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, id.ToString())
            }, "JwAuth"));
            return claimsPrincipal;
        }

        public async Task<int> GetClaimsFromJwt(string jwt)
        {
            if(jwt is null)
            {
                return -1;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var claims = token.Claims;
            var id = claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;

            return int.Parse(id);
        }
    }
}
