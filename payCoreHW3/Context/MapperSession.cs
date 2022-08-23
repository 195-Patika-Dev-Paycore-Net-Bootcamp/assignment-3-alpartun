﻿using System;
using NHibernate;
using payCoreHW3.Models;
using ISession = NHibernate.ISession;

namespace payCoreHW3.Context
{
    public class MapperSession : IMapperSession
    {
        private readonly ISession _session;
        private ITransaction _transaction;
        public MapperSession(ISession session)
        {
            _session = session;
        }

        public IQueryable<Vehicle> Vehicles => _session.Query<Vehicle>();
        public IQueryable<Container> Containers => _session.Query<Container>();

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            
        }

        public void CloseTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Save<T>(T entity) where T : class
        {
            _session.Save(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _session.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _session.Delete(entity);
        }

        /*public List<Vehicle> GetVehicles()
        {
            
        }

        public List<Container> GetContainers()
        {
            throw new NotImplementedException();
        }*/
    }
}

