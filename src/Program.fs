module GiraffeAngularTemplate.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.FileProviders
open FSharp.Control.Tasks.ContextSensitive
open Microsoft.AspNetCore.Http
// ---------------------------------
// Models
// ---------------------------------

type AngularLink = {
    name: string
    link: string
}

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine

    let ngAppTag = tag "app-root"  [] []
      
    let ngScripts = 
      [ "main.js"; "runtime.js"; "polyfills.js" ]
      |> List.map (function js -> script [ _type "text/javascript"; _src js ] [] )
      
    let index = 
      html [ _lang "en" ] [
          head [] [
              meta [ _charset "utf-8"; _name "viewport"; _content "width=device-width, initial-scale=1" ] 
              title [] [ str "Giraffe Angular App" ]
              ``base`` [ _href "/" ]
              link [ _rel "icon"; _type "icon"; _href "favicon.ico" ]
              link [ _rel "stylesheet"; _href "styles.css" ]]
  
          body [] 
              (ngAppTag :: ngScripts)
      ]

// ---------------------------------
// Http Handlers
// ---------------------------------

let angularLinksHandler: HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let result = [
                { name = "Tour of Heroes"; link = "https://angular.io/tutorial" }
                { name = "CLI Documentation"; link = "https://angular.io/cli" }
                { name = "Angular blog"; link = "https://angular.io/" }
            ]
            return! ctx.WriteJsonAsync<AngularLink list> result
        }

// ---------------------------------
// Web app
// ---------------------------------

let webApp =
    choose [
        GET >=> route "/" >=> htmlView Views.index
        subRoute "/api" (
            choose [
                GET >=> route "/angular-links" >=> angularLinksHandler
            ]
        )
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let angularStaticFilesOptions = 
    let path = Path.Combine(Directory.GetCurrentDirectory(), "client", "dist")
    let fileProvider = new PhysicalFileProvider(path)
    StaticFileOptions (FileProvider = fileProvider)

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles(angularStaticFilesOptions)
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0