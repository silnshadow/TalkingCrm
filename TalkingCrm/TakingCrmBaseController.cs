using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.AspNetCore.Mvc;
using TalkingCrm.DataProviderServices;
using TalkingCrm.Model;

namespace TalkingCrm
{
    [ApiController]
    [Route("[controller]")]
    public class TakingCrmBaseController : Controller
    {
        IBirthdayTrackerModel provider;
        public TakingCrmBaseController(IBirthdayTrackerModel p)
        {
            provider = p;
        }

        [HttpPost, Route("/process")]
        public SkillResponse Index(SkillRequest request)
        {
            SkillResponse response = new SkillResponse();
            response.Version = "1.0";
            response.Response = new ResponseBody();
            SkillResponse output = new SkillResponse();
            string userid = request.Session.User.UserId;
            switch (request.Request.Type)
            {
                case "LaunchRequest":
                    response.Response.OutputSpeech = new PlainTextOutputSpeech("I am listening. How can i help you");
                    response.Response.ShouldEndSession = false;

                    break;
                case "IntentRequest":
                    IntentRequest intentRequest = (IntentRequest)request.Request;
                    switch (intentRequest.Intent.Name)
                    {
                        case "next_birthday":
                            BirthdayTrackerModel b = provider.GetNextBirthday(userid);
                            if (b != null)
                            {
                                response.Response.OutputSpeech =
                                 new PlainTextOutputSpeech(String.Format("The next birthday is of {0} on {1}", b.Name, b.DateofBirthday.ToString("MMMM dd yyyy")));
                            }
                            else
                            {
                                response.Response.OutputSpeech =
                                 new PlainTextOutputSpeech("Sorry, No birthdays found!");
                            }
                            response.Response.ShouldEndSession = false;
                            break;
                        case "named_birthday":
                            string name = intentRequest.Intent.Slots["name"].SlotValue.Value;
                            BirthdayTrackerModel named = provider.GetBirthday(name);
                            if (named != null)
                            {
                                response.Response.OutputSpeech =
                                 new PlainTextOutputSpeech(String.Format("Birthday of {0} is on {1}", named.Name, named.DateofBirthday.ToString("MMMM dd yyyy")));
                            }
                            else
                            {
                                response.Response.OutputSpeech =
                                 new PlainTextOutputSpeech("Sorry, No birthdays found!");
                            }
                            response.Response.ShouldEndSession = false;
                            break;
                        case "add_new_birthday":

                            if (intentRequest.Intent.ConfirmationStatus == "DENIED")
                            {
                                response.Response.OutputSpeech =
                                 new PlainTextOutputSpeech("Ok, operation cancelled. Please try again");
                            }
                            else
                            {

                                BirthdayTrackerModel birthday = new BirthdayTrackerModel();
                                birthday.Name = intentRequest.Intent.Slots["name"].Value;
                                birthday.DateofBirthday = DateTime.Parse(intentRequest.Intent.Slots["birthday"].Value);
                                birthday.DayOfYear = birthday.DateofBirthday.DayOfYear;
                                bool success = provider.AddBirthday(birthday);
                                if (success)
                                {
                                    response.Response.OutputSpeech =
                                     new PlainTextOutputSpeech("Great! Birthday added!");
                                }
                                else
                                {
                                    response.Response.OutputSpeech =
                                     new PlainTextOutputSpeech("Sorry, could not add that birthday. Please try again");
                                }
                            }
                            response.Response.ShouldEndSession = false;
                            break;
                        case "AMAZON.StopIntent":
                            response.Response.OutputSpeech =
                                     new PlainTextOutputSpeech("Goodbye! See you soon.");
                            response.Response.ShouldEndSession = true;
                            break;
                        case "AMAZON.FallbackIntent":
                            response.Response.OutputSpeech =
                                new PlainTextOutputSpeech("Sorry! can you repeat that?");
                            response.Response.ShouldEndSession = false;
                            break;
                        default:
                            response.Response.OutputSpeech =
                                     new PlainTextOutputSpeech("Sorry! I am still learning that. Please try later!");
                            response.Response.ShouldEndSession = false;
                            break;

                    }
                    break;
            }
            return response;
        }
    }
}

  