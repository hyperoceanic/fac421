module fac421.Home

open Falco
open Falco.Markup
open SpotifyAPI.Web
open System

open SpotifyUtils
open Microsoft.AspNetCore.Http

let showUserProfile (userProfile : PrivateUser) =
    Elem.div [] [
        Elem.p [] [Text.raw userProfile.DisplayName]
        Elem.p [] [Text.raw userProfile.Email]
    ]
let showPlaylists (playlists : Paging<SimplePlaylist>) =

    let paras =
        playlists.Items
        |> Seq.map (fun (item) -> Elem.p [] [Text.raw item.Name])
        |> List.ofSeq

    Elem.div [] paras

let htmx =
    Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2";
    Attr.integrity "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h";
    Attr.crossorigin "anonymous" ] []

let loginPart =
    let uri = getLoginURI
    Elem.div [] [
        Elem.a [Attr.href (uri.ToString())] [Text.raw "Spotify Login"]
        Elem.p [] [Text.raw (uri.ToString())]
    ]

let spotifyPart spotify =
    let userProfile = getUserProfile spotify
    let playlists = getPlayLists spotify
    Elem.div [] [
        Elem.h2 [] [Text.raw userProfile.DisplayName]

        showUserProfile userProfile
        showPlaylists playlists
    ]
let tryGetCookieValue (context : HttpContext, key : string) =
    match context.Request.Cookies.TryGetValue key with
    | true, value -> Some (value.ToString())
    | _           -> None

let refreshTokenHandler (context : HttpContext) (args : AuthorizationCodeTokenResponse) =
    context.Response.Cookies.Append ("spotify_access_token", args.AccessToken)
    context.Response.Cookies.Append ("spotify_refresh_token", args.RefreshToken)

    printfn "%s" args.RefreshToken |> ignore


let homePageHandler : HttpHandler = fun ctx ->
    let spotify_code = tryGetCookieValue (ctx, "spotify_code")
    let tokenHandler = refreshTokenHandler ctx

    let html =
         Elem.html [ Attr.lang "en" ] [
            Elem.head [] []
            htmx
            Elem.body [] [
                Elem.h1 [] [ Text.raw "Fac 421" ]
                match spotify_code with
                | Some value ->
                    let spotify = getSpotify (tokenHandler, value)
                    spotifyPart spotify
                | None -> loginPart
            ]
        ]
    Response.ofHtml html ctx