using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using TrainCheck.Alexa;
using TrainCheck.Infrastructure;

namespace TrainCheck.Tests.EndToEnd
{
    public class TestFunction
    {
        public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context)
        {
            var alexaSkill = ServiceContainer.GetOrCreate<AlexaSkill>(s =>
            {
                s.AddTransient<IStandardHttpClient, MockHttpClient>();
            });

            return alexaSkill.Process(request, context);
        }
    }
}
