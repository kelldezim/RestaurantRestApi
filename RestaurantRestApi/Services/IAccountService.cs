using RestaurantRestApi.DtoModels;

namespace RestaurantRestApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto registerUserDto);
        string GenerateJwt(LoginDto dto);
    }
}
