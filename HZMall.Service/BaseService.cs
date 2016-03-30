using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HZMall.Core.Data;
using HZMall.Framework.Dapper;

namespace HZMall.Service
{

    public abstract class BaseService
    {
        protected readonly IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        protected readonly IBcDbContext _dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
        protected readonly ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        protected readonly DataUtils _dataDapper = BrEngineContext.Current.Resolve<DataUtils>();
    }
}