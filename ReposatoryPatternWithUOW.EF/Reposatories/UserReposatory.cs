using Microsoft.EntityFrameworkCore;
using ReposatoryPatternWithUOW.Core.DTOs;
using ReposatoryPatternWithUOW.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReposatoryPatternWithUOW.EF.Mapper;
using ReposatoryPatternWithUOW.Core.Models;
using Microsoft.Extensions.Options;
using ReposatoryPatternWithUOW.Core.ReturnedModels;
using ReposatoryPatternWithUOW.Core.OptionsPatternClasses;

namespace ReposatoryPatternWithUOW.EF.Reposatories
{
    public class UserReposatory:IUserReposatory
    {
        private readonly AppDbContext context;
        private readonly Mapperly mapper;
        private readonly ISenderService senderService;
        TokenOptionsPattern options;
        public UserReposatory(AppDbContext context, Mapperly mapper, TokenOptionsPattern options, ISenderService senderService)
        {
            this.context = context;
            this.mapper = mapper;
            this.options = options;
            this.senderService = senderService;
        }

        public async Task<bool> SignUpAsync(SignUpDto signupDto)
        {
            if (signupDto is null || await context.Users.AnyAsync(x => x.Email == signupDto.Email))
                return false;
            try
            {

                User user = mapper.MapToUser(signupDto);
                //user.EmailConfirmed = false;
                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
                user.Password = hashedPassword;
                await context.AddAsync(user);
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<LoginResult> LoginAsync(LoginDto loginDto)

        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            if (loginDto.Email is null || loginDto.Password is null)
                return new() { Success = false };

            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == loginDto.Email);
           
            if (user is null || !BCrypt.Net.BCrypt.EnhancedVerify(loginDto.Password, user.Password))
            {
                return new() { Success = false };
            }
            if (!user.EmailConfirmed)
            {
                return new()
                {
                    Success = true,
                    EmailConfirmed = false,
                };

            }
            var expirationOfJWT = DateTime.Now.AddMinutes(15);
            var expirationOfRefreshToken = DateTime.Now.AddHours(6);
            var refreshToken = new RefreshToken()
            {

                UserId = user.Id,
                CreatedAt = DateTime.Now,
                ExpiresAt = expirationOfRefreshToken,
                Token = TokenGenerator.GenerateToken()

            };
            context.Attach(user);
            user.RefreshTokens.Add(refreshToken);


            return new()
            {
                Success = true,
                EmailConfirmed = true,
                Jwt = TokenGenerator.GenerateToken(user, expirationOfJWT, options),
                ExpirationOfJwt = expirationOfJWT,
                ExpirationOfRefreshToken = expirationOfRefreshToken,
                RefreshToken = refreshToken.Token,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserId = user.Id

            };


        }

        public async Task<string?> SendVerficationCode(string email, bool? IsForResetingPassword = false)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            var user = await context.Users.AsNoTracking().Include(x => x.EmailVerificationCode).FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
                return null;
            if (user.EmailVerificationCode is not null && user.EmailVerificationCode.ExpiresAt < DateTime.Now.AddSeconds(-5))
                context.Remove(user.EmailVerificationCode);
            else if (user.EmailVerificationCode is not null)
            {
                return "sent";
            }
            var rand = new Random();
            var verificationNum = rand.Next(100000, int.MaxValue);
            user.EmailVerificationCode = new() { ExpiresAt = DateTime.Now.AddHours(1), Code = verificationNum.ToString() };
            context.Update(user);
            string body;
            string subject;
            if (IsForResetingPassword is null || IsForResetingPassword == false)
            {
                body = $"Dear {email} ,\n you have signed up on our Educationl application, \nand we have sent to you a verification code which is : <b>{verificationNum}</b> ";
                subject = "Email Confirmation";
            }
            else
            {
                body = $"Dear {email} ,\nThere was a request to reset your password on our Educationl application! \nIf you did not make this request then please ignore this email,\nand we have sent to you a verification code which is : <b>{verificationNum}</b> ";
                subject = "Reset Password";
            }
            Task t1, t2;
           
            await senderService.SendEmailAsync(email, subject, body);
            await context.SaveChangesAsync();

            return "ok";

        }

        public async Task<bool> ValidateCode(string email, string code, bool isForResetPassword = false)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;

            var user = await context.Users.Include(x => x.EmailVerificationCode).FirstOrDefaultAsync(x => x.Email == email);

            if (user is null || user.EmailVerificationCode is null )
                return false;

            if (user.EmailVerificationCode.Code != code)
            {

                if (user.EmailVerificationCode.ExpiresAt < DateTime.Now)
                {
                    context.Remove(user.EmailVerificationCode);
                }
                return false;
            }
            if (user.EmailVerificationCode.ExpiresAt < DateTime.Now)
            {
                context.Remove(user.EmailVerificationCode);
                return false;
            }
            if (!isForResetPassword)
            {
                user.EmailConfirmed = true;
                //context.Remove(user.IdentityTokenVerification);
                context.Update(user);
            }
            context.Remove(user.EmailVerificationCode);
            return true;

        }

        public async Task<bool> SignOut(string email,string token)
        {
            var result =context.RefreshTokens.Where(x => x.User.Email == email && x.Token == token).ExecuteDelete();
            return (result > 0);
           
        }
    }
}
