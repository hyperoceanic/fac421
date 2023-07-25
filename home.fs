module fac421.Home

open Falco
open Falco.Markup
open SpotifyUtils
open SpotifyAPI.Web
open System


let getLoginRequest =
    let env =  Configuration.GetSpotifyAppConfig
    let callbackUrl = Uri("https://localhost:5001/spotify/redirect")
    let scopes = [Scopes.PlaylistReadPrivate;  Scopes.PlaylistReadCollaborative] |> Array.ofSeq
    let request = LoginRequest( callbackUrl, env.spotify_client_id, LoginRequest.ResponseType.Code)
    request.Scope <- scopes
    request.ToUri()

let loginPart token =
    Elem.div [] [
        if token = "dunno"
        then
            let uri = getLoginRequest
            let x = uri.ToString()
            Elem.a [Attr.href x] [Text.raw "Spotify Login"]
        else
            Elem.p [] [Text.raw "Logged in"]
            let userProfile = GetUserProfile token
            Elem.p [] [Text.raw token]
            Elem.p [] [Text.raw userProfile.DisplayName]
            Elem.p [] [Text.raw userProfile.Email]
    ]

let homePageHandler : HttpHandler  = fun ctx ->
    let token  = ctx.Request.Cookies["code"]
    let html =
         Elem.html [ Attr.lang "en" ] [
            Elem.head [] []
            Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2";
                Attr.integrity "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h";
                Attr.crossorigin "anonymous" ] []
            Elem.body [] [
                Elem.h1 [] [ Text.raw "Fac 421" ]
                loginPart token
            ]
        ]
    Response.ofHtml html ctx