//programmed by: Himank Bansal
//u3183058
//Reference code1:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
//Reference Code2:https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.0

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// this is the data structure model for the data base what values will be stored and categorised as in squl table
/// the primary key inthis model is Token
/// </summary>
namespace API_101.Models
{
    public class APIModels
    {   
        public long Id { get; set; }

        public string LongUrl { get; set; }

        public string ShortUrl { get; set; }
        public string Token { get; set; }
    }

}
