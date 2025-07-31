using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task<UserResultDto> GetUserByEmail(string email);
        Task<bool> CheckIfEmailExist(string email);
        Task<AddressDto> UpdateAddressAsync(AddressDto addressDto, string email);
        Task<AddressDto> GetUserAddressAsync(string email);
    }
}
