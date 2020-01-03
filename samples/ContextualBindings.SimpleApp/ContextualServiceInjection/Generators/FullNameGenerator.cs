using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    public class FullNameGenerator : INameGenerator
    {
        private readonly INameGenerator _firstNameGenerator;
        private readonly INameGenerator _lastNameGenerator;

        public FullNameGenerator(INameGenerator firstNameGenerator, INameGenerator lastNameGenerator)
        {
            _firstNameGenerator = firstNameGenerator ?? throw new ArgumentNullException(nameof(firstNameGenerator));
            _lastNameGenerator = lastNameGenerator ?? throw new ArgumentNullException(nameof(lastNameGenerator));
        }

        public string Generate()
        {
            var firstName = _firstNameGenerator.Generate();
            var lastName = _lastNameGenerator.Generate();
            return $"{firstName} {lastName}";
        }
    }
}
