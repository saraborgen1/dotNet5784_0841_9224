﻿namespace BlImplementation;
using BlApi;
using BO;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IState State => new StateImplementation();
    public void InitializeDB() => DalTest.Initialization.Do();
    public void ResetDB() => DalTest.Initialization.Reset();
}
