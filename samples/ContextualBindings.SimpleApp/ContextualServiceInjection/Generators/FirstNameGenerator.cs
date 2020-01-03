using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    public class FirstNameGenerator : INameGenerator
    {
        private readonly Random _rng = new Random();
        private readonly string[] _names = new string[]
        {
            "Emma",
            "Olivia",
            "Ava"
        };

        public string Generate()
        {
            var index = _rng.Next(0, _names.Length);
            return _names[index];
        }
    }
}
