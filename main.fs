module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder

open configuration
open html
open spotify
open System.Collections.Generic
open System.Text.Json
open System.Web

let homePageHandler : HttpHandler = fun ctx ->

    let page = GetSpotifyAppConfig.spotify_client_id
            |> getLoginURI
            |> loginPage

    Response.ofHtml page ctx

let handlerWithHeader : HttpHandler =
    Response.withCookie "greeted" "1"
    >> Response.ofPlainText "Hello world"


let redirectPageHandler : HttpHandler = fun ctx ->
    let config = GetSpotifyAppConfig
    let auth = buildAuth config.spotify_client_id config.spotify_client_secret
    let code : string = ctx.Request.Query["code"][0]
    let payload = getAccessTokenM (auth, code)

    let dict = JsonSerializer.Deserialize<Dictionary<string, obj>>payload
    let access_token = dict["access_token"] |> string

    let page = spotifyPage access_token

    let bar = Response.withCookie "access_token" access_token
            >> Response.ofHtml page

    bar ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
    ]
}