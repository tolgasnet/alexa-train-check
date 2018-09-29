using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using TrainCheck.Alexa;

namespace TrainCheck.Tests.EndToEnd
{
    public class TestFunction
    {
        public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context)
        {
            var alexaSkill = ServiceContainer.GetOrCreate<AlexaSkill>();

            return alexaSkill.Process(request, context);
        }
    }
}
