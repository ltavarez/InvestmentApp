using InvestmentApp.Core.Domain.Interfaces;
using InvestmentApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvestmentApp.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity>
        where Entity : class
    {
        private readonly InvestmentAppContext _context;
        private readonly ILogger<GenericRepository<Entity>> _logger;
        public GenericRepository(InvestmentAppContext context, ILogger<GenericRepository<Entity>> logger)
        {
            _context = context;
            _logger = logger;
        }
        public virtual async Task<Entity?> AddAsync(Entity entity)
        {
            _logger.LogInformation("Adding new entity of type {EntityType}", typeof(Entity).Name);
            await _context.Set<Entity>().AddAsync(entity);
            _logger.LogInformation("Entity added successfully, saving changes to the database");
            await _context.SaveChangesAsync();
            _logger.LogInformation("Entity of type {EntityType} added successfully", typeof(Entity).Name);
            return entity;
        }
        public virtual async Task<List<Entity>?> AddRangeAsync(List<Entity> entities)
        {
            await _context.Set<Entity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
        public virtual async Task<Entity?> UpdateAsync(int id, Entity entity)
        {
            _logger.LogInformation("Updating entity of type {EntityType} with ID: {Id}", typeof(Entity).Name, id);
            var entry = await _context.Set<Entity>().FindAsync(id);

            _logger.LogInformation("Found entity: {Entity}", entry);
            if (entry != null)
            {
                _logger.LogInformation("Updating entity with new values");
                _context.Entry(entry).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return entry;
            }

            _logger.LogWarning("Entity with ID {Id} not found for update", id);
            return null;
        }
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Entity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<Entity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public virtual async Task<List<Entity>> GetAllList()
        {
            return await _context.Set<Entity>().ToListAsync(); //EF - immediate execution
        }

        public virtual async Task<List<Entity>> GetAllListWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync(); //EF - immediate execution
        }
        public virtual async Task<Entity?> GetById(int id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
        public virtual IQueryable<Entity> GetAllQuery()
        {
            return _context.Set<Entity>().AsQueryable();//select * from assetsType // where join //deferred execution
        }
        public virtual IQueryable<Entity> GetAllQueryWithInclude(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return query; //EF - deffered execution
        }
    }
}