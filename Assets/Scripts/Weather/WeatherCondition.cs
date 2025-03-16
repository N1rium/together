using System;

namespace Weather
{
    [Serializable]
    public class WeatherCondition
    {
        public WeatherType weatherType;
        public RenderLayer renderLayer;
    }
    
    public enum WeatherType
    {
        None = 0,
        Rain = 1,
    }

    public enum RenderLayer
    {
        Background = 0,
        Default = 1,
        Foreground = 2,
    }
}