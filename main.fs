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
    let code = ctx.Response.StatusCode
    let accessToken = getAccessTokenM code
    Response.ofHtml (spotifyPage code) ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
    ]
}