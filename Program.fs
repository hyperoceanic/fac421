module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup

open SpotifyUtils
open SpotifyAPI.Web

let htmlHandler : HttpHandler =
    let html =
        Elem.html [ Attr.lang "en" ] [
            Elem.head [] []
            Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2"; Attr.integrity  "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h"; Attr.crossorigin "anonymous" ] []
            Elem.body [] [
                Elem.h1 [] [ Text.raw "Sample App" ]
                Elem.button [Attr.id "login"; Attr.create "hx-get" "/spotify/login"] [Text.raw "Login to Spotify"]
            ]
        ]
    Response.ofHtml html

let login =
    let env = Configuration.GetSpotifyAppConfig
    let token = getToken env
    Response.ofPlainText token.AccessToken

webHost [||]  {
    use_static_files
    endpoints [
        get "/" (Response.ofPlainText ("Hello world" ));
        get "/spotify" htmlHandler
        get "/spotify/login" login
    ]
}