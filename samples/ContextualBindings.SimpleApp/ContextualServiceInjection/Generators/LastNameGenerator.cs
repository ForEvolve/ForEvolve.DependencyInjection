using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    public class LastNameGenerator : INameGenerator
    {
        private readonly Random _rng = new Random();
        private readonly string[] _names = new string[]
        {
            "Wang",
            "Devi",
            "Zhang"
        };

        public string Generate()
        {
            var index = _rng.Next(0, _names.Length);
            return _names[index];
        }
    }
}
