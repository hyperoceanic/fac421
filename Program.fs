module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup

open SpotifyUtils
open Main
open Home

open Microsoft.AspNetCore.WebUtilities
open SpotifyAPI.Web
open System

let redirectHandler : HttpHandler  = fun ctx ->
    let env =  Configuration.GetSpotifyAppConfig
    let q = Request.getQuery ctx
    let code = q.GetString  ("code", "dunno")

    let client = SpotifyAPI.Web.OAuthClient()
    let originatingUri = Uri("https://localhost:5001/spotify/redirect")
    let request = AuthorizationCodeTokenRequest(env.spotify_client_id,
        env.spotify_client_secret, code, originatingUri)
    let response = client.RequestToken(request)

    let token = response.Result.AccessToken

    ctx.Response.Cookies.Append ("token", token) |> ignore
    // Response.ofPlainText code ctx

    Response.redirectTemporarily "/" ctx

webHost [||]  {
    use_static_files
    endpoints [
        get "/" homePageHandler
        // get "/spotify/login" login
        get "/spotify/redirect" redirectHandler
        // get "/spotify/main" mainHandler
     ]
}