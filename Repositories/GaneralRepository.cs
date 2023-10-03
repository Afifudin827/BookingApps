﻿using Server.Contracts;
using Server.Data;

namespace Server.Repositories;

public class GaneralRepository <TEntity> : IGaneralRepository<TEntity> where TEntity : class
{
    private readonly BookingManagementDbContext _context;
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
        catch
        {
            return null;
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
        catch
        {
            return false;
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
        catch
        {
            return false;
        }
    }
}   
