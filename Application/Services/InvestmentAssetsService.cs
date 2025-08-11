using AutoMapper;
using AutoMapper.QueryableExtensions;
using InvestmentApp.Core.Application.Dtos.InvestmentAssets;
using InvestmentApp.Core.Application.Interfaces;
using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Core.Application.Services
{
    public class InvestmentAssetsService : GenericService<InvestmentAssets, InvestmentAssetsDto>, IInvestmentAssetsService
    {
        private readonly IInvestmentAssetRepository _investmentAssetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestmentAssetsService> _logger;
        public InvestmentAssetsService(IInvestmentAssetRepository investmentAssetRepository, IMapper mapper, ILoggerFactory loggerFactory)
            : base(investmentAssetRepository, mapper, loggerFactory.CreateLogger<InvestmentAssetsService>())
        {
            _investmentAssetRepository = investmentAssetRepository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<InvestmentAssetsService>();
        }
        public async Task<InvestmentAssetsDto?> GetByAssetAndPortfolioAsync(int assetId, int portfolioId, string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving InvestmentAsset by AssetId: {AssetId} and PortfolioId: {PortfolioId} for UserId: {UserId}", assetId, portfolioId, userId);
                var investmentAsset = await _investmentAssetRepository
                    .GetAllQueryWithInclude(["Asset"])
                    .FirstOrDefaultAsync(ia => ia.AssetId == assetId
                    && ia.InvestmentPortfolioId == portfolioId
                     && ia.InvestmentPortfolio != null
                && ia.InvestmentPortfolio.UserId == userId);

                _logger.LogInformation("InvestmentAsset retrieval completed. AssetId: {AssetId}, PortfolioId: {PortfolioId}, UserId: {UserId}", assetId, portfolioId, userId);
                if (investmentAsset == null)
                {
                    _logger.LogWarning("No InvestmentAsset found for AssetId: {AssetId}, PortfolioId: {PortfolioId}, UserId: {UserId}", assetId, portfolioId, userId);
                    return null;
                }

                _logger.LogInformation("Mapping InvestmentAsset to InvestmentAssetsDto");
                InvestmentAssetsDto dto = _mapper.Map<InvestmentAssetsDto>(investmentAsset);

                _logger.LogInformation("Returning InvestmentAssetsDto for AssetId: {AssetId}, PortfolioId: {PortfolioId}, UserId: {UserId}", assetId, portfolioId, userId);
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<InvestmentAssetsDto?> GetById(int id, string userId)
        {
            try
            {
                var listEntitiesQuery = _investmentAssetRepository.GetAllQueryWithInclude(["Asset", "InvestmentPortfolio"]);

                var entity = await listEntitiesQuery.FirstOrDefaultAsync(
                fd => fd.Id == id
                && fd.InvestmentPortfolio != null
                && fd.InvestmentPortfolio.UserId == userId);

                if (entity == null)
                {
                    return null;
                }

                var dto = _mapper.Map<InvestmentAssetsDto>(entity);

                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<InvestmentAssetsDto>> GetAllWithInclude(string userId)
        {
            try
            {
                var listEntitiesQuery = _investmentAssetRepository.GetAllQueryWithInclude(["Asset", "InvestmentPortfolio"]);
                listEntitiesQuery = listEntitiesQuery
                        .Where(w => w.InvestmentPortfolio != null && w.InvestmentPortfolio.UserId == userId);

                var listEntityDtos = await listEntitiesQuery.ProjectTo<InvestmentAssetsDto>(_mapper.ConfigurationProvider).ToListAsync();

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}