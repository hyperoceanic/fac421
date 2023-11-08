module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder

open configuration
open html
open spotify

let homePageHandler : HttpHandler = fun ctx ->

    let page = GetSpotifyAppConfig.spotify_client_id
            |> getLoginURI
            |> loginPage

    Response.ofHtml page ctx

let redirectPageHandler : HttpHandler = fun ctx ->
    let config = GetSpotifyAppConfig
    let auth = buildAuth config.spotify_client_id config.spotify_client_secret
    let code : string = ctx.Request.Query["code"][0]
    let accessToken = getAccessTokenM (auth, code)
    Response.ofHtml (spotifyPage accessToken) ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
    ]
}