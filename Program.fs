module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup

open SpotifyUtils

open Microsoft.AspNetCore.WebUtilities
let getLoginUri: string  =
    let env =  Configuration.GetSpotifyAppConfig
    let url = GetLoginUserUri (env)
    url.ToString()

let htmlHandler =
    let url : string = getLoginUri
    let html =
        Elem.html [ Attr.lang "en" ] [
            Elem.head [] []
            Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2"; Attr.integrity  "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h"; Attr.crossorigin "anonymous" ] []
            Elem.body [] [
                Elem.h1 [] [ Text.raw "Sample App" ]
                Elem.p [] [Text.raw url]
                Elem.a [Attr.href url] [Text.raw "Spotify Login"]
            ]
        ]
    Response.ofHtml html
let login =
    Configuration.GetSpotifyAppConfig
    |> getToken
    |> fun x -> x.AccessToken
    |> Response.ofPlainText

let redirectHandler : HttpHandler  = fun ctx ->
    let q = Request.getQuery ctx
    let code = q.GetString  ("code", "dunno")

    Response.withHeaders [  "token", code ] ctx |> ignore
    Response.ofPlainText code ctx

webHost [||]  {
    use_static_files
    endpoints [
        get "/" (Response.ofPlainText ("Hello world" ));
        get "/spotify" htmlHandler
        get "/spotify/login" login
        get "/spotify/redirect" redirectHandler
    ]
}