using System;

namespace Shared
{
    public class Temperature
    {
        public Temperature(double temperature)
        {
            Celsius = temperature;
        }

        public double Celsius { get; }
        public double Fahrenheit => (Celsius * 9) / 5 + 32;
        public double Kelvin => Celsius + 273.15;
    }
}
