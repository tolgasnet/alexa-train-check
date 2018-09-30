using System;
using TrainCheck.Infrastructure;
using TrainCheck.TransportApi;

namespace TrainCheck.Tests.Component
{
    public class MockHttpClient : IStandardHttpClient
    {
        private static TrainResponse _expectedResponse;

        public T GetAsync<T>(Uri uri) where T : class
        {
            return _expectedResponse as T;
        }

        public static void SetSuccessfulResponse()
        {
            Create(
                new Departure(expected: "13:00", aimed: "13:00"),
                new Departure(expected: "13:15", aimed: "13:15"),
                new Departure(expected: "13:45", aimed: "13:45"));
        }

        public static void SetCancelledResponse()
        {
            Create(
                new Departure(expected: "13:00", aimed: "13:00"),
                new Departure(expected: null, aimed: "13:15", status: "CANCELLED"),
                new Departure(expected: "13:45", aimed: "13:45"));
        }

        private static void Create(params Departure[] departures)
        {
            _expectedResponse = new TrainResponse
            {
                Departures = new Departures
                {
                    All = departures
                }
            };
        }
    }
}
