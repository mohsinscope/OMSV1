# OMSV1
dotnet ef migrations add DocumentNull --project OMSV1.Infrastructure --startup-project OMSV1.Application
dotnet ef database update --project OMSV1.Infrastructure --startup-project OMSV1.Application


dotnet publish -c Release -o ./publish   

dotnet ef Migrations list --project OMSV1.Infrastructure --startup-project OMSV1.Application     

development 

"DefaultConnection": "Host=localhost;Port=5432;Database=OMSDBV1GuidV3;Username=postgres;Password=postgres" ,
"HangfireConnection": "Host=localhost;Port=5432;Database=OMSDBV1GuidV3;Username=postgres;Password=postgres" 

    // Create roles
    // var roles = new[] { "Admin", "Supervisor", "Manager"};
    // foreach (var role in roles)
    // {
    //     if (!await roleManager.RoleExistsAsync(role))
    //     {
    //         await roleManager.CreateAsync(new AppRole { Name = role });
    //     }
    // }

    // Create an admin user
    // var adminEmail = "admin";
    // var adminUser = await userManager.FindByEmailAsync(adminEmail);
    // if (adminUser == null)
    // {
    //     var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
    //     await userManager.CreateAsync(user, "Admin@123");
    //     await userManager.AddToRoleAsync(user, "Admin");
    // }



 

"ConnectionStrings": {
"DefaultConnection": "Host=172.16.108.27;Port=5432;Database=stage-DB;Username=stage-User;Password='admin@123'" ,
"HangfireConnection": "Host=172.16.108.27;Port=5432;Database=stage-DB;Username=stage-User;Password='admin@123'"   
},