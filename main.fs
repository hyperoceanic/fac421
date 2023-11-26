module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder

open configuration
open html
open spotify
open System.Collections.Generic
open System.Text.Json

let homePageHandler : HttpHandler = fun ctx ->

    let page = GetSpotifyAppConfig.spotify_client_id
            |> getLoginURI
            |> loginPage

    Response.ofHtml page ctx

let redirectPageHandler : HttpHandler = fun ctx ->
    let config = GetSpotifyAppConfig
    let auth = buildAuth config.spotify_client_id config.spotify_client_secret
    let code : string = ctx.Request.Query["code"][0]
    let payload = getAccessTokenM (auth, code)

    let dict = JsonSerializer.Deserialize<Dictionary<string, obj>>payload
    let access_token = dict["access_token"] |> string

    let page = spotifyPage access_token

    let response = Response.withCookie "access_token" access_token
                >> Response.ofHtml page

    response ctx

let playlistsHandler : HttpHandler = fun ctx ->
    let accessToken = ctx.Request.Cookies["access_token"]
    let playlists = getPlaylists accessToken
    let fragment = playlistsFragment playlists
    let response = Response.ofHtml fragment
    response ctx

let playlistHandler: HttpHandler =
    fun ctx ->
        let accessToken = ctx.Request.Cookies["access_token"]
        let route = Request.getRoute ctx
        let Id = route.GetString "Id"
        let playList = getPlaylist accessToken Id
        let fragment = html.playlist playList
        let response = Response.ofHtml fragment
        response ctx

let playAlbumHandler: HttpHandler =
    fun ctx ->
        let accessToken = ctx.Request.Cookies["access_token"]
        let route = Request.getRoute ctx
        let Id = route.GetString "Id"
        let result = playAlbum accessToken Id
        let response = Response.ofHtmlString "<p>OK</p>"
        response ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
        get "/playlists" playlistsHandler
        get "/playlist/{Id}" playlistHandler
        get "/play/{Id}" playAlbumHandler
    ]
}