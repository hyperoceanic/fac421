module fac421.Program

open System.Net.Http
open Microsoft.AspNetCore.Http

open Falco
open Falco.Routing
open Falco.HostBuilder

open configuration
open html
open spotify
open System.Collections.Generic
open System.Text.Json

let HomePageHandler : HttpHandler = fun ctx ->

    let response = GetSpotifyAppConfig.spotify_client_id
                |> getLoginURI
                |> loginPage
                |> Response.ofHtml
    response ctx

let RedirectPageHandler : HttpHandler = fun ctx ->
    let config = GetSpotifyAppConfig
    let auth = buildAuth config.spotify_client_id config.spotify_client_secret
    let code : string = ctx.Request.Query["code"][0]
    let payload = requestAccessToken (auth, code)

    let dict = JsonSerializer.Deserialize<Dictionary<string, obj>> payload
    let access_token = dict["access_token"] |> string

    let page = spotifyPage access_token

    let response = Response.withCookie "access_token" access_token
                >> Response.ofHtml page

    response ctx

let AccessToken (ctx : HttpContext)  =
    ctx.Request.Cookies["access_token"]

let RouteValue ctx key =
    let route = Request.getRoute ctx
    route.GetString key

let DevicesHandler : HttpHandler = fun ctx ->
    let response = ctx
                |> AccessToken
                |> getDevices
                |> devicesFragment
                |> Response.ofHtml
    response ctx

let PlaylistsHandler : HttpHandler = fun ctx ->
    let response = ctx
                |> AccessToken
                |> getPlaylists
                |> playlistsFragment
                |> Response.ofHtml
    response ctx

let PlaylistHandler: HttpHandler = fun ctx ->
    let Id = RouteValue ctx "Id"
    let accessToken = AccessToken ctx

    let response = (accessToken, Id)
                 ||> getPlaylist
                  |> html.playlistView
                  |> Response.ofHtml
    response ctx

let PlayAlbumHandler: HttpHandler = fun ctx ->
    let Id = RouteValue ctx "Id"
    let accessToken = AccessToken ctx

    let response = playAlbum accessToken Id
                  |> Response.ofHtmlString
    response ctx

webHost [||] {
    endpoints [
        get "/" HomePageHandler
        get "/spotify" RedirectPageHandler
        get "/devices" DevicesHandler
        get "/playlists" PlaylistsHandler
        get "/playlist/{Id}" PlaylistHandler
        get "/play/{Id}" PlayAlbumHandler
    ]
}