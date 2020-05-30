using System;

namespace ForEvolve.DependencyInjection
{
    public class ScanningContextInitializedException : Exception
    {
        public ScanningContextInitializedException()
            : base($"The ScanningContext was already initialized.")
        {

        }
    }
}
