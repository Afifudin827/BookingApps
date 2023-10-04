using Server.Contracts;
using Server.Data;
using Server.Utilities.Handler;

namespace Server.Repositories;

public class GaneralRepository <TEntity> : IGaneralRepository<TEntity> where TEntity : class
{
    protected readonly BookingManagementDbContext _context;
    protected GaneralRepository(BookingManagementDbContext context)
    {
        _context = context;
    }
    /*
     * Pada bagian Repositorynya juga akan di buat sebuah 
     * class general yang nantinya akan digunakan pada 
     * setiap repository class lainnya karena memiliki fitur yang sama yaitu CRUD.
     */
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    public TEntity? GetByGuid(Guid guid)
    {
        var entity = _context.Set<TEntity>().Find(guid);
        _context.ChangeTracker.Clear();
        return entity;
    }

    public TEntity? Create(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_tb_m_employees_nik"))
            {
                throw new ExceptionHandler("NIK already exists");
            }
            if (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_tb_m_employees_email"))
            {
                throw new ExceptionHandler("Email already exists");
            }
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_tb_m_employees_phone_number"))
            {
                throw new ExceptionHandler("Phone number already exists");
            }
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public bool Update(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    public bool Delete(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }
}   
