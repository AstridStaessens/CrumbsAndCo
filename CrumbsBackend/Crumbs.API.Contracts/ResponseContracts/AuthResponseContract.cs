namespace Crumbs.API.Contracts.ResponseContracts
{
    public class AuthResponseContract
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}