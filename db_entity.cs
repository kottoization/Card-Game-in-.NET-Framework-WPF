using System;
using System.Data.Entity;
using System.Linq;

namespace probny
{
    public class db_entity : DbContext
    {
        // Your context has been configured to use a 'db_entity' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'probny.db_entity' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'db_entity' 
        // connection string in the application configuration file.
        public db_entity()
            : base("name=db_entity")
        {
        }

        public virtual DbSet<Gracz> GraczBaza { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}