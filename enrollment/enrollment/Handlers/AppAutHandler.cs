using enrollment.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace enrollment.Handlers
{
    public class AppAutHandler :
        AuthenticationHandler<AuthenticationSchemeOptions>
    {
       // private Services.IStudentsDbService _service;


        public AppAutHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock clock) : base(options, loggerFactory, urlEncoder, clock)
        {
           
        }
           

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("nie podales parametrow");
            }
            var aut = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(aut.Parameter);
            string[] dane = Encoding.UTF8.GetString(bytes).Split(":");
            
            if (dane.Length == 2)
            {
                if(CheckLoginData(dane[0], dane[1]))
                
                {
                    var claims = new[] { new Claim(ClaimTypes.Role, "student"), new Claim(ClaimTypes.Name, dane[0]) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            return AuthenticateResult.Fail("Cos poszlo nie tak");


        }
        
        private bool CheckLoginData(string index, string password)
        {
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18508;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = client;
                client.Open();
                com.CommandText = "select IndexNumber , haslo from Student where IndexNumber=@index AND haslo=@haslo";
                com.Parameters.AddWithValue("index", index);
                com.Parameters.AddWithValue("haslo", password);
                var dr = com.ExecuteReader();
                if (!dr.Read()) // jesli brak rekordow to zla autoryzacja
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
        
    }
}
