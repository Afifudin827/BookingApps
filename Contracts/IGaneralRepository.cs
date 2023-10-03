namespace Server.Contracts;

public interface IGaneralRepository<TEntity> where TEntity : class
{
    /*
     * Pada bagian interface membuat sebuah general yang nantinya 
     * akan di gunakan pada setiap table interface karena setiap table nantinya akan dapat melakukan 
     * GetAll, GetByGuid, Create, Update dan Delete.
     */
    IEnumerable<TEntity> GetAll();
    TEntity? GetByGuid(Guid guid);
    TEntity? Create(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
}
