namespace Presentation
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
        {
            var user = await serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(user);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
        {
            var user = await serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(user);
        }
        [HttpPost("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await serviceManager.AuthenticationService.CheckIfEmailExist(email);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult<UserResultDto>> GetUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await serviceManager.AuthenticationService.GetUserByEmail(email);
            return Ok(user);
        }
        [Authorize]
        [HttpGet("GetUserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await serviceManager.AuthenticationService.GetUserAddressAsync(email);
            return Ok(address);
        }
        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var updatedAddress = await serviceManager.AuthenticationService.UpdateAddressAsync(address, email);
            return Ok(updatedAddress);
        }

    }
}
