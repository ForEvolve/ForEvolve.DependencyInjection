using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextualBindings.SimpleApp.ComplexObjectTree
{
    [Route("/api/complex-object-tree")]
    public class ComplexObjectTreeController : ControllerBase
    {
        private readonly IComplexObjectTreeService _partialSetupService;

        public ComplexObjectTreeController(IComplexObjectTreeService partialSetupService)
        {
            _partialSetupService = partialSetupService ?? throw new ArgumentNullException(nameof(partialSetupService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _partialSetupService.Get();
            return Ok(result);
        }
    }

    public interface IComplexObjectTreeService
    {
        IEnumerable<string> Get();
    }

    public class ComplexObjectTreeService : IComplexObjectTreeService
    {
        private readonly IDirectDependency _dependency1;
        private readonly IDirectDependency _dependency2;

        public ComplexObjectTreeService(IDirectDependency dependency1, IDirectDependency dependency2)
        {
            _dependency1 = dependency1 ?? throw new ArgumentNullException(nameof(dependency1));
            _dependency2 = dependency2 ?? throw new ArgumentNullException(nameof(dependency2));
        }

        public IEnumerable<string> Get()
        {
            return _dependency1
                .Get()
                .Concat(_dependency2.Get())
            ;
        }
    }

    public interface IDirectDependency
    {
        IEnumerable<string> Get();
    }

    public class DirectDependency1 : IDirectDependency
    {
        private readonly ISubDependency1 _dependency1;
        private readonly ISubDependency2 _dependency2;
        private readonly ISubDependency3 _dependency3;

        public DirectDependency1(ISubDependency1 dependency1, ISubDependency2 dependency2, ISubDependency3 dependency3)
        {
            _dependency1 = dependency1 ?? throw new ArgumentNullException(nameof(dependency1));
            _dependency2 = dependency2 ?? throw new ArgumentNullException(nameof(dependency2));
            _dependency3 = dependency3 ?? throw new ArgumentNullException(nameof(dependency3));
        }

        public IEnumerable<string> Get()
        {
            yield return _dependency1.Get();
            yield return _dependency2.Get();
            yield return _dependency3.Get();
        }
    }

    public class DirectDependency2 : IDirectDependency
    {
        private readonly ISubDependency1 _dependency1;
        private readonly ISubDependency2 _dependency2;
        private readonly ISubDependency3 _dependency3;

        public DirectDependency2(ISubDependency1 dependency1, ISubDependency2 dependency2, ISubDependency3 dependency3)
        {
            _dependency1 = dependency1 ?? throw new ArgumentNullException(nameof(dependency1));
            _dependency2 = dependency2 ?? throw new ArgumentNullException(nameof(dependency2));
            _dependency3 = dependency3 ?? throw new ArgumentNullException(nameof(dependency3));
        }

        public IEnumerable<string> Get()
        {
            yield return _dependency1.Get();
            yield return _dependency2.Get();
            yield return _dependency3.Get();
        }
    }

    public interface ISubDependency1
    {
        string Get();
    }
    public interface ISubDependency2
    {
        string Get();
    }
    public interface ISubDependency3
    {
        string Get();
    }

    public class SubDependency1_1 : ISubDependency1
    {
        public string Get() => nameof(SubDependency1_1);
    }
    public class SubDependency1_2 : ISubDependency1
    {
        public string Get() => nameof(SubDependency1_2);
    }
    public class SubDependency2_1 : ISubDependency2
    {
        public string Get() => nameof(SubDependency2_1);
    }
    public class SubDependency2_2 : ISubDependency2
    {
        public string Get() => nameof(SubDependency2_2);
    }
    public class SubDependency3_1 : ISubDependency3
    {
        public string Get() => nameof(SubDependency3_1);
    }
    public class SubDependency3_2 : ISubDependency3
    {
        public string Get() => nameof(SubDependency3_2);
    }
}
