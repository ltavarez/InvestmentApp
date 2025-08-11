using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using InvestmentApp.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Persistence.Repositories
{
    public class AssetTypeRepository : GenericRepository<AssetType>, IAssetTypeRepository
    {
        public AssetTypeRepository(InvestmentAppContext context, ILoggerFactory loggerFactory) 
            : base(context, loggerFactory.CreateLogger<AssetTypeRepository>())
        {
        }
    }
}