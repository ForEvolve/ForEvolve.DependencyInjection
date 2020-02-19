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
        private readonly IComplexObjectTreeService _complexObjectTreeService;

        public ComplexObjectTreeController(IComplexObjectTreeService complexObjectTreeService)
        {
            _complexObjectTreeService = complexObjectTreeService ?? throw new ArgumentNullException(nameof(complexObjectTreeService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _complexObjectTreeService.Get();
            return Ok(result);
        }
    }

    public interface IComplexObjectTreeService
    {
        object Get();
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

        public object Get()
        {
            return new
            {
                name = nameof(ComplexObjectTreeService),
                dependency1 = _dependency1.Get(),
                dependency2 = _dependency2.Get()
            };
        }
    }

    public interface IDirectDependency
    {
        object Get();
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

        public object Get()
        {
            return new
            {
                name = nameof(DirectDependency1),
                dependency1 = _dependency1.Get(),
                dependency2 = _dependency2.Get(),
                dependency3 = _dependency3.Get()
            };
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

        public object Get()
        {
            return new
            {
                name = nameof(DirectDependency2),
                dependency1 = _dependency1.Get(),
                dependency2 = _dependency2.Get(),
                dependency3 = _dependency3.Get()
            };
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
