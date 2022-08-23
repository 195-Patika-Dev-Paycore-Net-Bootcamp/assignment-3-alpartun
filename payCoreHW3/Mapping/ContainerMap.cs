using System;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using payCoreHW3.Models;

namespace payCoreHW3.Mapping
{
    public class ContainerMap : ClassMapping<Container>
    {
        public ContainerMap() 
        {
        // public virtual long Id { get; set; }
        // public virtual string ContainerName { get; set; } 
        // public virtual decimal Latitude { get; set; }
        // public virtual decimal Longitude { get; set; }
        // public virtual long VehicleId { get; set; }
            Table("container");

            Id(x => x.Id,
                x =>
                {
                    x.Type(NHibernateUtil.Int64);
                    x.UnsavedValue(0);
                    x.Generator(Generators.Increment);
                } );
            Property(x => x.ContainerName,
                x =>
                {
                    x.Length(50);
                    x.Type(NHibernateUtil.String);
                });
            Property(x => x.Latitude,
                x =>
                {
                    x.Type(NHibernateUtil.Currency);
                    x.Precision(10);
                    x.Scale(6);
                });
            Property(x => x.Longitude,
                x =>
                {
                    x.Type(NHibernateUtil.Currency);
                    x.Precision(10);
                    x.Scale(6);
                });
            Property(x => x.VehicleId,
                x =>
                {
                    x.Type(NHibernateUtil.Int64);
                });


        }
    }
}

