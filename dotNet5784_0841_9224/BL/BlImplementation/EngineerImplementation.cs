﻿namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;


internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Engineer item)
    {
        if (item.Id <= 0)throw new ArgumentException();
        if(item.Name==null)throw new ArgumentException();
        if(item.Cost<=0) throw new ArgumentException();
        if (item.Email != null)
        {
            if (!item.Email.Contains("@")) throw new ArgumentException();
            if (!item.Email.Contains(".")) throw new ArgumentException();
        }

        DO.Engineer doEngineer = new DO.Engineer
            (item.Id,item.Name,item.Email,item.Level,item.Cost);

        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={item.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(p => p.Id == id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        BO.TaskInEngineer taskInEngineer = null!;

        DO.Task? temp = _dal.Task.Read(p => p.EngineerId == doEngineer.Id);
        if (temp != null)
            taskInEngineer = new TaskInEngineer() { Id = temp.Id, Alias = temp.Ailas };

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEngineer
        };
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool> filter = null!
    {
        var doEngineerList=(from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
            select new BO.Engineer()
            {

                Id = doEngineer.Id,
                Name = doEngineer.Name,
                Email = doEngineer.Email,
                Level = doEngineer.Level,
                Cost = doEngineer.Cost,
                Task = new TaskInEngineer()
                {
                    Id = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Id,
                    Alias = _dal.Task.Read(p => p.EngineerId == doEngineer.Id)!.Ailas
                }
            }).ToList();

        if (filter != null)
            return (from item in doEngineerList
                    where filter(item)
                    select item).ToList();

        return doEngineerList;
    }

    public void Update(Engineer item)
    {
        throw new NotImplementedException();
    }
}