using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using InvestmentApp.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Persistence.Repositories
{
    public class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {      
        public AssetRepository(InvestmentAppContext context, ILoggerFactory loggerFactory) 
            : base(context, loggerFactory.CreateLogger<AssetRepository>())
        {            
        }
    }
}