using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Xunit;

namespace TrainCheck.Tests.Component
{
    public class FunctionTests
    {
        private readonly TestFunction _function;
        private readonly SkillRequest _anyLaunchRequest;

        public FunctionTests()
        {
            _function = new TestFunction();
            _anyLaunchRequest = Create(new LaunchRequest());
        }

        [Fact]
        public void FavouriteRouteOnTime()
        {
            MockHttpClient.SetSuccessfulResponse();

            var response = _function.FunctionHandler(_anyLaunchRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to blackfriars are at: 13:00, 13:15, 13:45</speak>");
        }

        [Fact]
        public void SelectedRouteOnTime()
        {
            MockHttpClient.SetSuccessfulResponse();

            var intentRequest = CreateIntent("victoria");

            var response = _function.FunctionHandler(intentRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to victoria are at: 13:00, 13:15, 13:45</speak>");
        }

        [Fact]
        public void SelectedRouteCancelled()
        {
            MockHttpClient.SetCancelledResponse();

            var intentRequest = CreateIntent("victoria");

            var response = _function.FunctionHandler(intentRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to victoria are at: 13:00, 13:15 CANCELLED, 13:45</speak>");
        }

        [Fact]
        public void SelectedRouteDuplicate()
        {
            MockHttpClient.SetDuplicateResponse();

            var intentRequest = CreateIntent("victoria");

            var response = _function.FunctionHandler(intentRequest, new TestLambdaContext());

            var speech = response.Response.OutputSpeech as SsmlOutputSpeech;

            speech.Should().NotBeNull();
            speech.Ssml.Should().Be("<speak>Your next trains to victoria are at: 13:00, 13:45, 14:00</speak>");
        }

        private SkillRequest Create(Request request)
        {
            return new SkillRequest
            {
                Request = request,
                Session = new Session
                {
                    Application = new Application(),
                    User = new User()
                }
            };
        }

        private SkillRequest CreateIntent(string destination)
        {
            return Create(new IntentRequest
            {
                Intent = new Intent
                {
                    Name = "livetimetableintent",
                    Slots = new Dictionary<string, Slot>
                    {
                        {
                            "Stations", new Slot
                            {
                                Value = destination
                            }
                        }
                    }
                }
            });
        }
    }
}
