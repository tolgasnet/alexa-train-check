using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using TrainCheck.Alexa;
using TrainCheck.Infrastructure;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TrainCheck
{
    public class Function
    {
        public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context)
        {
            var alexaSkill = ServiceContainer.GetOrCreate<AlexaSkill>();

            return alexaSkill.Process(request, context);
        }
    }
}
