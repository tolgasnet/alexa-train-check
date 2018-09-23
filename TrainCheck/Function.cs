using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using TrainCheck.Alexa;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TrainCheck
{
    public class Function
    {
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            App.Logger = context.Logger;

            var alexaSkill = ServiceContainer.GetOrCreate<AlexaSkill>();

            return alexaSkill.Process(input);
        }
    }
}
