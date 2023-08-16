using System;
namespace AspNetCore6WebAPI.Services
{
	public class LocalMailService
	{
		private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";

		public void Send(string subject, string message)
		{
			Console.WriteLine($"Mail from {_mailFrom} ton {_mailTo}, " + $"with {nameof(LocalMailService)}");
			Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Subject: {message}");
        }
    }
}

