using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Xunit;

namespace TrainCheck.Tests.EndToEnd
{
    public class FunctionTests
    {
        private readonly TestFunction _function;
        private readonly SkillRequest _anyLaunchRequest;

        public FunctionTests()
        {
            _function = new TestFunction();
            _anyLaunchRequest = new SkillRequest
            {
                Request = new LaunchRequest(),
                Session = new Session
                {
                    Application = new Application(),
                    User = new User()
                }
            };
        }

        [Fact]
        public void LaunchTest()
        {
            var response = _function.FunctionHandler(_anyLaunchRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to blackfriars are at: 13:00, 13:15, 13:45</speak>");
        }

        [Fact]
        public void LiveTimetableIntentTest()
        {
            var intentRequest = new SkillRequest
            {
                Request = new IntentRequest
                {
                    Intent = new Intent
                    {
                        Name = "livetimetableintent",
                        Slots = new Dictionary<string, Slot>
                        {
                            {"Stations", new Slot
                            {
                                Value = "victoria"
                            }}
                        }
                    }
                },
                Session = new Session
                {
                    Application = new Application(),
                    User = new User()
                }
            };

            var response = _function.FunctionHandler(intentRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to victoria are at: 13:00, 13:15, 13:45</speak>");
        }
    }
}
