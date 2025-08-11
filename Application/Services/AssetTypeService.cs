using AutoMapper;
using AutoMapper.QueryableExtensions;
using InvestmentApp.Core.Application.Dtos.AssetType;
using InvestmentApp.Core.Application.Interfaces;
using InvestmentApp.Core.Domain.Entities;
using InvestmentApp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Core.Application.Services
{
    public class AssetTypeService : GenericService<AssetType, AssetTypeDto>, IAssetTypeService
    {
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetTypeService> _logger;
        public AssetTypeService(IAssetTypeRepository assetTypeRepository, IMapper mapper, ILoggerFactory loggerFactory)
            : base(assetTypeRepository, mapper, loggerFactory.CreateLogger<AssetTypeService>())
        {
            _assetTypeRepository = assetTypeRepository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<AssetTypeService>();
        }
        public async Task<List<AssetTypeDto>> GetAllWithInclude()
        {
            try
            {
                _logger.LogInformation("Retrieving all AssetTypes with included Assets");
                var listEntitiesQuery = _assetTypeRepository.GetAllQueryWithInclude(["Assets"]);

                _logger.LogInformation("Projecting AssetType entities to DTOs");
                var listEntityDtos = await listEntitiesQuery.ProjectTo<AssetTypeDto>(_mapper.ConfigurationProvider).ToListAsync();

                _logger.LogInformation("Returning list of AssetType DTOs");
                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
