using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enrollment.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string sciezka = httpContext.Request.Path; //"weatherforecast/cos"
                string queryStr = httpContext.Request?.QueryString.ToString();
                string metoda = httpContext.Request.Method.ToString();
                string bodyStr = "";

                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                //logowanie do pliku
                FileStream fout = null;
                if (File.Exists("requestLog.txt")){ 
                    fout = new FileStream("requestLog.txt", FileMode.Append);
                }
                else
                {
                    fout = new FileStream("requestLog.txt", FileMode.Create);
                }
                var fstr = new StreamWriter(fout);
                fstr.Write("Metoda: " + metoda + "\nSciezka: " + sciezka + "\nQueryString: " + queryStr + "\nBody: " + bodyStr + "\n");
                fstr.Close();
                fout.Close();
            }
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext); 

        }

    }
}
