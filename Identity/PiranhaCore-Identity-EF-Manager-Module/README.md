## Welcom GyreTech PiranhaCMS Identity EF Manager Module
PiranhaCMS Module that adds .NET Core Identity User Management UI to manage the Piranha Identity EF Module.

## Registering the Module

    public IServiceProvider ConfigureServices(IServiceCollection services) {
		....
		
		// Add Identity Security EF Manager
        services.AddEfIdentityManager();

		....

	}

## Initialize the Module
Once the Module has been configured you initialize it. Please note that generic AspNetCore setup has been omitted.

	public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)  {
		....
    
		// Initialize the Identity EF Manager module
		app.UseEfIdentityManager();
    
		....
    }