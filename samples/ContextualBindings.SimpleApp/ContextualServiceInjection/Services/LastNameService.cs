using System;

namespace ContextualBindings.SimpleApp.ContextualServiceInjection
{
    public class LastNameService : ILastNameService
    {
        private readonly INameGenerator _nameGenerator;
        public LastNameService(INameGenerator nameGenerator)
        {
            _nameGenerator = nameGenerator ?? throw new ArgumentNullException(nameof(nameGenerator));
        }

        public string GetLastName()
        {
            return _nameGenerator.Generate();
        }
    }
}
