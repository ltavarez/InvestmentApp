using AutoMapper;
using AutoMapper.QueryableExtensions;
using InvestmentApp.Core.Application.Dtos.InvestmentPortfolio;
using InvestmentApp.Core.Application.Interfaces;
using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Core.Application.Services
{
    public class InvestmentPortfolioService : GenericService<InvestmentPortfolio, InvestmentPortfolioDto>, IInvestmentPortfolioService
    {
        private readonly IInvestmentPortfolioRepository _investmentPortfolioRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentPortfolioService> _logger;
        public InvestmentPortfolioService(IInvestmentPortfolioRepository investmentPortfolioRepository, IMapper mapper, ILoggerFactory loggerFactory) 
            : base(investmentPortfolioRepository, mapper, loggerFactory.CreateLogger<InvestmentPortfolioService>())
        {
            _investmentPortfolioRepository = investmentPortfolioRepository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<InvestmentPortfolioService>();
        }       
        public override async Task<InvestmentPortfolioDto?> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving InvestmentPortfolio by Id: {Id}", id);
                var listEntitiesQuery = _investmentPortfolioRepository.GetAllQuery();

                _logger.LogInformation("Executing query to find InvestmentPortfolio with Id: {Id}", id);
                var entity = await listEntitiesQuery.FirstOrDefaultAsync(a => a.Id == id);

                _logger.LogInformation("InvestmentPortfolio retrieval completed. Id: {Id}", id);
                if (entity == null)
                {
                    _logger.LogWarning("No InvestmentPortfolio found for Id: {Id}", id);
                    return null;
                }

                _logger.LogInformation("Mapping InvestmentPortfolio to InvestmentPortfolioDto");
                var dto = _mapper.Map<InvestmentPortfolioDto>(entity);

                _logger.LogInformation("Returning InvestmentPortfolioDto for Id: {Id}", id);
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<InvestmentPortfolioDto?> GetById(int id,string userId)
        {
            try
            {
                var listEntitiesQuery = _investmentPortfolioRepository.GetAllQuery();
                var entity = await listEntitiesQuery.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (entity == null)
                {
                    return null;
                }

                var dto = _mapper.Map<InvestmentPortfolioDto>(entity);
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<InvestmentPortfolioDto>> GetAllWithIncludeByUser(string userId)
        {
            try
            {
                var listEntitiesQuery = _investmentPortfolioRepository.GetAllQuery()
                    .Where(ip=>ip.UserId == userId);

                var listEntityDtos = await listEntitiesQuery.ProjectTo<InvestmentPortfolioDto>(_mapper.ConfigurationProvider).ToListAsync();            

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}