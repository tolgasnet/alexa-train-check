﻿using Amazon.Lambda.Core;

namespace TrainCheck
{
    public static class Logger
    {
        public static ILambdaLogger Instance { private get; set; }

        public static void Log(string message)
        {
            Instance.LogLine(message);
        }
    }
}
