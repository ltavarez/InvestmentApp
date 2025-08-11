using InvestmentApp.Core.Application.Exceptions;
using InvestmentApp.Core.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace InvestmentApp.Core.Application.Features.Assets.Commands.CreateAsset
{
    /// <summary>
    /// Parameters required to create a new asset
    /// </summary>
    public class CreateAssetCommand : IRequest<int>
    {
        /// <example>Bitcoin</example>
        [SwaggerParameter(Description = "The name of the asset to be created")]
        public string? Name { get; set; }

        /// <example>Criptomoneda descentralizada líder en el mercado</example>
        [SwaggerParameter(Description = "A brief description of the asset (optional)")]
        public string? Description { get; set; }

        /// <example>BTC</example>
        [SwaggerParameter(Description = "The trading symbol or abbreviation of the asset")]
        public string? Symbol { get; set; }

        /// <example>1</example>
        [SwaggerParameter(Description = "The ID of the asset type (foreign key reference)")]
        public int AssetTypeId { get; set; }
    }

    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, int>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger _createAssetCommandHandlerLogger;
        public CreateAssetCommandHandler(IAssetRepository assetRepository, ILoggerFactory loggerFactory)
        {
            _assetRepository = assetRepository;
            _createAssetCommandHandlerLogger = loggerFactory.CreateLogger<CreateAssetCommandHandler>();
        }

        public async Task<int> Handle(CreateAssetCommand command, CancellationToken cancellationToken)
        {
            _createAssetCommandHandlerLogger.LogInformation("Creating asset with Name: {Name}, Description: {Description}, Symbol: {Symbol}, AssetTypeId: {AssetTypeId}",
                command.Name, command.Description, command.Symbol, command.AssetTypeId);
            Domain.Entities.Asset entity = new()
            {
                Id = 0,
                Name = command.Name ?? "",
                Description = command.Description,
                AssetTypeId = command.AssetTypeId,
                Symbol = command.Symbol ?? ""                
            };

            _createAssetCommandHandlerLogger.LogInformation("Asset entity created with Id: {Id}, Name: {Name}, Description: {Description}, Symbol: {Symbol}, AssetTypeId: {AssetTypeId}",
                entity.Id, entity.Name, entity.Description, entity.Symbol, entity.AssetTypeId);
            Domain.Entities.Asset? result = await _assetRepository.AddAsync(entity);

            _createAssetCommandHandlerLogger.LogInformation("Asset creation result: {Result}", result != null ? "Success" : "Failure");
            return result == null ? throw new ApiException("Error created assets", (int)HttpStatusCode.InternalServerError) : result.Id;
        }
    }
}
