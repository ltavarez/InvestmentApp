using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using InvestmentApp.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Persistence.Repositories
{
    public class InvestmentPortfolioRepository : GenericRepository<InvestmentPortfolio>, IInvestmentPortfolioRepository
    {      
        public InvestmentPortfolioRepository(InvestmentAppContext context, ILoggerFactory loggerFactory) 
            : base(context, loggerFactory.CreateLogger<InvestmentPortfolioRepository>())
        {            
        }
    }
}