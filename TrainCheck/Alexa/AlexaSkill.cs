using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace TrainCheck.Alexa
{
    public class AlexaSkill
    {
        private readonly IAlexaSpeech _alexaSpeech;

        public AlexaSkill(IAlexaSpeech alexaSpeech)
        {
            _alexaSpeech = alexaSpeech;
        }

        public SkillResponse Process(SkillRequest input, ILambdaContext context)
        {
            Logger.Instance = context.Logger;

            var requestType = input.GetRequestType();
            Logger.Log(
                $"Received request, type: {requestType.Name}, " +
                $"applicationid:{input.Session.Application.ApplicationId}, " +
                $"userid:{input.Session.User.UserId}");

            if (requestType == typeof(LaunchRequest))
            {
                return _alexaSpeech.TellTimeTable();
            }

            if (requestType == typeof(IntentRequest))
            {
                return IntentResponse(input.Request as IntentRequest);
            }

            return null;
        }

        private SkillResponse IntentResponse(IntentRequest intentRequest)
        {
            var intentName = intentRequest.Intent.Name.ToLower();

            Logger.Log($"Received intent request {intentName}");

            if (intentName == "livetimetableintent")
            {
                var destinationStationName = intentRequest.Intent.Slots["Stations"].Value;

                return _alexaSpeech.TellTimeTable(destinationStationName);
            }

            return ResponseBuilder.Empty();
        }
    }
}