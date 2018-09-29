using Amazon.XRay.Recorder.Core;

namespace TrainCheck
{
    public static class Xray
    {
        public static void Begin(string key)
        {
            if (!ServiceContainer.IsXRayEnabled()) return;

            AWSXRayRecorder.Instance.BeginSubsegment(key);
        }

        public static void End()
        {
            if (!ServiceContainer.IsXRayEnabled()) return;

            AWSXRayRecorder.Instance.EndSubsegment();
        }
    }
}