using AutoMapper;
using InvestmentApp.Core.Application.Interfaces;
using InvestmentApp.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Core.Application.Services
{
    public class GenericService<Entity , DtoModel> : IGenericService<DtoModel>
        where Entity : class
        where DtoModel : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GenericService<Entity, DtoModel>> _logger;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper, ILogger<GenericService<Entity, DtoModel>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public virtual async Task<DtoModel?> AddAsync(DtoModel dto)
        {
            try
            {
                _logger.LogInformation("Adding new entity of type {EntityType}", typeof(Entity).Name);
                Entity entity = _mapper.Map<Entity>(dto);

                _logger.LogInformation("Mapped DTO to entity: {Entity}", entity);
                Entity? returnEntity = await _repository.AddAsync(entity);

                _logger.LogInformation("Entity added successfully, returning mapped DTO");
                if (returnEntity == null)
                {
                    _logger.LogWarning("Failed to add entity of type {EntityType}", typeof(Entity).Name);
                    return null;
                }

                _logger.LogInformation("Adding new entity of type {EntityType}", typeof(Entity).Name);
                return _mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<DtoModel?> UpdateAsync(DtoModel dto, int id)
        {
            try
            {
                Entity entity = _mapper.Map<Entity>(dto);
                Entity? returnEntity = await _repository.UpdateAsync(id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual async Task<DtoModel?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetById(id);
                if (entity == null)
                {
                    return null;
                }

                DtoModel dto = _mapper.Map<DtoModel>(entity);
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<List<DtoModel>> GetAll()
        {
            try
            {
                var listEntities = await _repository.GetAllList();
                var listEntityDtos = _mapper.Map<List<DtoModel>>(listEntities);

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }     
    }
}