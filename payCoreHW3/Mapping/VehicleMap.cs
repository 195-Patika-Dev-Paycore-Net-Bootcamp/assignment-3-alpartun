using System;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using payCoreHW3.Models;

namespace payCoreHW3.Mapping
{
    public class VehicleMap : ClassMapping<Vehicle>
    {
        public VehicleMap()
        {
            Table("vehicle");

            Id(x => x.Id,
                x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            } );
            Property(x => x.VehicleName,
                x =>
                {
                    x.Length(50);
                    x.Type(NHibernateUtil.String);
                });
            Property(x => x.VehiclePlate,
                x =>
                {
                    x.Length(14);
                    x.Type(NHibernateUtil.String);
                });
        }
    }
}

