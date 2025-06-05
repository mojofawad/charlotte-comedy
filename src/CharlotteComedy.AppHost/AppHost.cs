var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("pg-username", "l0c@l-D3v-user");
var password = builder.AddParameter("pg-pass", "l0c@l-D3v-pass");

var pgsql = builder.AddPostgres("pgsql", username, password);

var writeDb = pgsql.AddDatabase("write-db");
var readDb = pgsql.AddDatabase("read-db");

var writeService = builder.AddProject<Projects.CharlotteComedy>("command-api")
    .WithReference(writeDb);

var readService = builder.AddProject<Projects.CharlotteComedy_Query>("query-api")
    .WithReference(readDb);

var gateway = builder.AddProject<Projects.CharlotteComedy_Gateway>("api-gateway")
    .WithReference(writeService)
    .WithReference(readService);

builder.AddNpmApp("charlotte-comedy-web", "../charlotte-comedy-web", "dev")
    .WithReference(gateway)
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();