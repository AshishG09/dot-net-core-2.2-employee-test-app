﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Here you can also use AddDBContext class instead of 
            services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            /*
             * Adding authorization globally instead of adding [Authorize] for every controller
             * We may exclude actions that do not require authorization by adding [AllowAnonymous] attribute on that
             * action
             */
            services.AddMvc(config => {
                //create authorization policy
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                //Add above policy to the filter
                config.Filters.Add(new AuthorizeFilter(policy));
                
            }).AddXmlSerializerFormatters(); // To be able to return data in xml format


            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>(); //This is dependency injection for IEmployee interface 

            //Adding services for ASP.NET Core Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<AppDBContext>();

            /*Configure default password complexity
               We can use below method to customize default passord complexity
               However, we are doing this above in the AddIdentity menthod itself
             */
            //services.Configure<IdentityOptions>(options => {
            //    options.Password.RequiredLength = 5;
            //    options.Password.RequireNonAlphanumeric = false;
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                /*
                 Below middleware handles unhandled exceptions like a Global exception handler
                 */
                app.UseExceptionHandler("/Error");

                /*
                 Below redirect pipline redirects the not found paths to Error/404 page,
                 this redirect is a 302 redirect (resource temporarily moved) which reflects in the browser.
                 The requested Error/404 page also reflects in the browser as a 200 OK. Thus the actual 404 error
                 that occured on the server does not reflect in the browser. Therefore, this method of showing 
                 error page is not semantically correct and should be avoided.
                 */
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");

                /*
                 Below pipeline method re-executes the Http request and does not do a 302 redirect. Thus the URL in the 
                 browser does not change even when the error page is displayed. Also the 200 OK message sent by the MVC 
                 peice of pipline is replaced by the 404 status by the re-execute method to restore the original status
                 code. Thus, the browser correctly displays the status message. Below method also allows us to access the
                 original url path, querystring etc as seen in the Error 404 page.
                 */
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");

            app.UseStaticFiles();

            /*Add authentication after adding Identity*/
            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute(); Commented to demonstrate routing in ASP.NET MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.Run(async (context) =>

            //{            
            //    await context.Response.WriteAsync("Hello World!");           
            //});
        }
    }
}
