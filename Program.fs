module fac421.Program
open Falco
open Falco.Routing
open Falco.HostBuilder

open Home

let redirectHandler : HttpHandler  = fun ctx ->

    let query = Request.getQuery ctx
    let code = query.GetString ("code", "dunno")
    ctx.Response.Cookies.Append ("spotify_code", code)

    Response.redirectTemporarily "/" ctx

webHost [||]  {
    use_static_files
    endpoints [
        get "/" homePageHandler
        get "/spotify/redirect" redirectHandler
     ]
}