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

let homePageHandler : HttpHandler = fun ctx ->

    let response = GetSpotifyAppConfig.spotify_client_id
                |> getLoginURI
                |> loginPage
                |> Response.ofHtml
    response ctx

let redirectPageHandler : HttpHandler = fun ctx ->
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

let getAccessToken (ctx : HttpContext)  =
    ctx.Request.Cookies["access_token"]

let getRouteValue ctx key =
    let route = Request.getRoute ctx
    route.GetString key


let devicesHandler : HttpHandler = fun ctx ->
    let response = ctx
                |> getAccessToken
                |> getDevices
                |> devicesFragment
                |> Response.ofHtml
    response ctx

let playlistsHandler : HttpHandler = fun ctx ->
    let response = ctx
                |> getAccessToken
                |> getPlaylists
                |> playlistsFragment
                |> Response.ofHtml
    response ctx

let playlistHandler: HttpHandler = fun ctx ->
    let Id = getRouteValue ctx "Id"
    let accessToken = getAccessToken ctx

    let response = (accessToken, Id)
                 ||> getPlaylist
                  |> html.playlistView
                  |> Response.ofHtml
    response ctx

let playAlbumHandler: HttpHandler = fun ctx ->
    let Id = getRouteValue ctx "Id"
    let accessToken = getAccessToken ctx

    let response = playAlbum accessToken Id
                  |> Response.ofHtmlString
    response ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
        get "/devices" devicesHandler
        get "/playlists" playlistsHandler
        get "/playlist/{Id}" playlistHandler
        get "/play/{Id}" playAlbumHandler
    ]
}