## Welcome GyreTech PiranhaCMS Identity EF Module
PiranhaCMS Module that handles ASP.NET Core Identity with Entity Framework as the backing store.

## Registering the Module

You register the Module by hooking it into the services. Please note that generic AspNetCore & Piranha setup has been omitted. However you should register the Module after your other Piranha CMS configurations.

    public IServiceProvider ConfigureServices(IServiceCollection services) {
		.... 
		// Add addtional all available claims
		List<string[]> additionalClaims = new List<string[]>() { Piranha.Manager.Permission.All() };

		// Add Identity Security EF
		services.AddEfIdentitySecurity(o =>
		{
			o.ConnectionString = defaultConnection;
			o.InitialClaims = additionalClaims;
			o.Users = new[]
			{
				new Piranha.AspNetCore.Identity.EF.Data.EfIdentityUser(true, Permission.All())
				{
					UserName = "Admin",
					Password = "P@sswOrd1",
					FirstName = "Admin",
					LastName = "User",
				}
			};
			o.EnableFirstLastNameClaim = true;
		});
		....
    }

## Initialize the Module

Once the Module has been configured you initialize it. Please note that generic AspNetCore setup has been omitted.

	public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)  {
		....
    
		// Initialize the Identity EF module
		app.UsePiranhaIdentityEFSecurity();
    
		....
    }



