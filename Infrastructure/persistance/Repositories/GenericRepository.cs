namespace Persistance.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)=> await _context.Set<TEntity>().AddAsync(entity);

        public async Task<int> CountAsync(Specifications<TEntity> specifications)
            => await ApplySpecifications(specifications).CountAsync();
        
        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool AsNoTracking = false) => AsNoTracking?
           await  _context.Set<TEntity>().AsNoTracking().ToListAsync() :
           await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _context.Set<TEntity>().FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications)
         => await ApplySpecifications(specifications).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(Specifications<TEntity> specifications)
            => await ApplySpecifications(specifications).FirstOrDefaultAsync();

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        private IQueryable<TEntity> ApplySpecifications(Specifications<TEntity> specifications)
            => SpecificationEvaluator.GetQuery<TEntity>(_context.Set<TEntity>(), specifications);
    }
}
