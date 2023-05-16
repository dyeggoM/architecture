using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.Generics.Data;
using TEST.Architectures.Generics.Entities;

namespace TEST.Architectures.Generics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiGenericsController<T> : ApiBaseController<T> where T : BaseEntity, new()
    {
        public ApiGenericsController(ApplicationContext context) : base(context) { }
    }

    public class DbModelController : ApiGenericsController<DbModel>
    {
        public DbModelController(ApplicationContext context) : base(context) { }

    }

    public class TestModelController : ApiGenericsController<TestModel>
    {
        public TestModelController(ApplicationContext context) : base(context) { }

    }
}