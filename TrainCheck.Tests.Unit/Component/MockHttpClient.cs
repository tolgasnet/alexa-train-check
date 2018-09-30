using System;
using TrainCheck.Infrastructure;
using TrainCheck.TransportApi;

namespace TrainCheck.Tests.Component
{
    public class MockHttpClient : IStandardHttpClient
    {
        public T GetAsync<T>(Uri uri) where T : class
        {
            return SuccessfulResponse() as T;
        }

        private TrainResponse SuccessfulResponse()
        {
            return new TrainResponse
            {
                Departures = new Departures
                {
                    All = new[]
                    {
                        new Departure
                        {
                            ExpectedDepartureTime = "13:00"
                        }, 
                        new Departure
                        {
                            ExpectedDepartureTime = "13:15"
                        },
                        new Departure
                        {
                            ExpectedDepartureTime = "13:45"
                        },
                    }
                }
            };
        }
    }
}
