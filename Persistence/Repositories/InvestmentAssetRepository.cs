using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using InvestmentApp.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Persistence.Repositories
{
    public class InvestmentAssetRepository : GenericRepository<InvestmentAssets>, IInvestmentAssetRepository
    {      
        public InvestmentAssetRepository(InvestmentAppContext context, ILoggerFactory loggerFactory) 
            : base(context, loggerFactory.CreateLogger<InvestmentAssetRepository>())
        {            
        }
    }
}