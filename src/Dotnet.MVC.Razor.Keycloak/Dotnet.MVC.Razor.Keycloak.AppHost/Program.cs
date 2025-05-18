var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Dotnet_MVC_Razor_Keycloak>("dotnet-mvc-razor-keycloak");

builder.Build().Run();
