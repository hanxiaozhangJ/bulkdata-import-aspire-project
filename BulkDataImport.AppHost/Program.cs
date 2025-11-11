var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL container + pgAdmin + database
var pgServer = builder.AddPostgres("postgres")
    .WithPgAdmin();
    
var db = pgServer.AddDatabase("EntityBulkImport", databaseName: "EntityBulkImport");

// Add migration project - it will wait for PostgreSQL to be ready and receive connection string
var migration = builder.AddProject<Projects.BulkDataImport_Migration>("migration")
    .WithReference(db);

builder.Build().Run();
