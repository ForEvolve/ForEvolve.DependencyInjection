using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    public class FirstNameService : IFirstNameService
    {
        private readonly INameGenerator _nameGenerator;
        public FirstNameService(INameGenerator nameGenerator)
        {
            _nameGenerator = nameGenerator ?? throw new ArgumentNullException(nameof(nameGenerator));
        }

        public string GetFirstName()
        {
            return _nameGenerator.Generate();
        }
    }
}
