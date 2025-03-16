using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoApi;



public class NoOpEmailSender : IEmailSender<IdentityUser>
{
    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
    {
        return Task.CompletedTask;
    }
}