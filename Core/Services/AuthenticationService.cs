namespace Services
{
    class AuthenticationService(UserManager<User> _userManager, IOptions<JwtOptions> options, IMapper mapper) : IAuthenticationService
    {
        public async Task<bool> CheckIfEmailExist(string email)
        {
            var user= await _userManager.FindByEmailAsync(email);
            return user is not null; //if user is not null, email already exists
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(u => u.Adress)
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            return mapper.Map<AddressDto>(user.Adress);
        }

        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UserNotFoundException(email);
            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            //Email is already addded to the database
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnauthorizedException(); //User not found 

            //Password is correct
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new UnauthorizedException(); //Password is incorrect

            return new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }

            return  new UserResultDto(user.DisplayName, await CreateTokenAsync(user), user.Email);
        }
        public async Task<AddressDto> UpdateAddressAsync(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users.Include(u => u.Adress)
                            .FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundException(email);
            if (user.Adress is not null)
            {
                user.Adress.FirstName = addressDto.FirstName;
                user.Adress.LastName = addressDto.LastName;
                user.Adress.Street = addressDto.Street;
                user.Adress.City = addressDto.City;
                user.Adress.Country = addressDto.Country;
            }
            else
            {
                var newAddress = mapper.Map<Adress>(addressDto); 
                user.Adress = newAddress;
            }
            await _userManager.UpdateAsync(user);
            return mapper.Map<AddressDto>(user.Adress);
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = options.Value;
            //private claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var siginCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpirationInDays),
                signingCredentials: siginCreds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
