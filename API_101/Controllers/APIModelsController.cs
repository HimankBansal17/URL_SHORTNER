//programmed by: Himank Bansal
//u3183058
//Reference code1:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio
//Reference Code2:https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.0


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_101.Models;

namespace API_101.Controllers
{

    [ApiController]
    public class APIModelsController : ControllerBase
    {

        private readonly API_Context _context;


        public double CacheTimeout { get; private set; }


        // To store an instance of current data in the database
        public APIModelsController(API_Context context)
        {
            _context = context;
        }

        //Get request To list all the data on the web page
        [Route("[Controller]")]
        // GET: api/APIModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<APIModels>>> GetAPIModel()
        {
            return await _context.APIModel.ToListAsync();
        }

        
        
        //Get request to take the token from the request and redirect the short url to original web page
        [HttpGet]
        [Route("{Token}")]
        public async Task<ActionResult<APIModels>> GetAPIModels(string Token)
        {
            APIModels aPIModels = await _context.APIModel.FindAsync(Token);

            if (aPIModels == null)
            {
                return NotFound();
            }
            string n = aPIModels.LongUrl;
            return RedirectPermanent(n);//returns 301 status code and reditrects to the page
        }


        //To check if the Url entered already has a token if (yes:then do not add it to 
        //data base  by returning**** HTTP CODE 200 *and return on the page with short url and token)
        private APIModels CheckExistingUrl(string LongUrl)
        {
            IEnumerable<APIModels> aPIModels = _context.APIModel.ToList();
            for(int i=0;i<aPIModels.Count();i++)
            {
                APIModels dbmodel = aPIModels.ElementAt(i);
                if (dbmodel.LongUrl == LongUrl)
                    {
                        return dbmodel ;
                    }
            }
            return null;
        }

        //put request if  in future the app needs a function to edit the data in database
        // PUT: api/APIModels/5
        [HttpPut("{id}")]
        [Route("[Controller]")]
        public async Task<IActionResult> PutAPIModels(long id, APIModels aPIModels)
        {
            if (id != aPIModels.Id)
            {
                return BadRequest();
            }
            
            _context.Entry(aPIModels).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!APIModelsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //function to generate the token of 7 digits with each diggit being different from one another
        private string Codegen()
        {
            char[] codegen = new char[7];
            int i = 0;
            bool repeating = false;
            bool complete = false;
            
            //do while lopp to generate code of 7 digit
            //it runs until the array of 7 is completed
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
                char[] digits= chars.ToCharArray();
                Random v = new Random();
                int Value = v.Next(0, digits.Length);
                
                
                for (int j = 0; j <i; j++)              //loop to check if the generated alphabet is already stored in the array or not
                {
                    if (digits[Value] == codegen[j])// if it is stored then set repeating to true and break the loop
                    {
                        repeating = true;
                        break;
                    }
                    
                }
                if (repeating != true)//if the the lopp finishes without break then there is no repeatation and the generated value can be stored in the array
                {
                    codegen[i] = digits[Value];

                    if (i == 6)// to exit the while loop 
                    {
                        complete = true;
                    }
                    else
                    {
                        i++;//if the array is not completly filled then incriment the index and continue the loop
                    }
                }
                else//if the lopp breaks with repeating ==true then reset the valeu to check the repetation for the next code
                {
                    repeating = false;
                }
            } while (complete != true);

            string codegen_str = new string(codegen);// convert the given character array to string
            return codegen_str;
        }

        // POST: api/APIModels
       /// <summary>
       /// this is a post request setuo fo r the api
       /// when the user makes a post request to add url it checksthe following:
       ///          1.if the entered URl is valid or not
       ///              =>if yes:then check if the url already exists in the database
       ///                  =>if yest :the url is in data base then requtrn with a code if http status code 200 and the object holding the url
       ///                  =>if no:then generate the token for the url
       ///                      =>if the token already exists
       ///                          =>if yes:regenerate a token
       ///                          =>if no: add the token to the database with the given url
       ///              =>if no: return with response of HTTp status code 400 bad request
       ///          
       /// </summary>
        string link = "https://localhost:44366/";
        [HttpPost]
        [Route("[Controller]")]
        public async Task<ActionResult<APIModels>> PostAPIModels(APIModels aPIModels)
        {
            bool tokenexisting = false;
            string codegen_str;
            bool valid=ValidateUrl(aPIModels.LongUrl);
            if(valid==true)
            {
                APIModels status = CheckExistingUrl(aPIModels.LongUrl);
                if (status !=null)
                {
                    return status;
                }
                else
                {
                   // char[] name = aPIModels.LongUrl.ToCharArray();
                    do
                    {
                         codegen_str= Codegen();
                        tokenexisting = CheckExistingToken(codegen_str);
                    } while (tokenexisting == true);
                    
                    aPIModels.Token = codegen_str;
                    aPIModels.ShortUrl =link + aPIModels.Token;
                    _context.APIModel.Add(aPIModels);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetAPIModel), new { aPIModels.Id }, aPIModels);
                }
            }
            else
            {
                return BadRequest();
            }
        }


        //function to check if the existing token
        private bool CheckExistingToken(string token)
        {
            IEnumerable<APIModels>  list = _context.APIModel.ToList();
            bool ExistingToken = false;
            for (int i = 0; i < list.Count(); i++)
            {
                APIModels dbmodel = list.ElementAt(i);
                if (dbmodel.Token== token)
                {
                    return ExistingToken=true ;
                }
            }
            return ExistingToken;
        }


        //Delete reuquest setup for use in future if the WebPage needs to edit the data in database of the api server
       // DELETE: api/APIModels/5
        [HttpDelete]
        [Route("[Controller]/{id}")]

        public async Task<ActionResult<APIModels>> DeleteAPIModels(long id)
        {
            var aPIModels = await _context.APIModel.FindAsync(id);
            if (aPIModels == null)
            {
                return aPIModels;
            }

            _context.APIModel.Remove(aPIModels);
            await _context.SaveChangesAsync();

            return aPIModels;
        }

        private bool APIModelsExists(long id)
        {
            return _context.APIModel.Any(e => e.Id == id);
        }


        //function to validate the url using the urishcheme
        private bool ValidateUrl(string url)
        {
            Uri validatedUri;

            if (Uri.TryCreate(url, UriKind.Absolute, out validatedUri)) //.NET URI validation.
            {
                //If true: validatedUri contains a valid Uri. Check for the scheme in addition.
                return (validatedUri.Scheme == Uri.UriSchemeHttp || validatedUri.Scheme == Uri.UriSchemeHttps);
            }
            return false;
        }
    }


}
