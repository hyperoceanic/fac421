module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Falco.Markup

let htmlHandler : HttpHandler =
    let config = Configuration.GetSpotifyAppConfig
    let html =
        Elem.html [ Attr.lang "en" ] [
            Elem.head [] []
            Elem.script [ Attr.src "/spotify.js"; Attr.type' "text/javascript" ] []
            Elem.body [] [
                Elem.h1 [] [ Text.raw "Sample App" ]
                Elem.p [] [Text.raw ("Client ID: " + config.spotify_client_id)]
                Elem.button  [] [Text.raw "Login2"]
                Elem.button [Attr.onclick "foo();" ] [Text.raw "Login"]
            ]
        ]

    Response.ofHtml html

let login : HttpHandler =
    let config = Configuration.GetSpotifyAppConfig
    let token = Spotify.LogInApp config
    Response.ofPlainText "token"

[<EntryPoint>]
let main args =
    webHost args {
         use_static_files;

        endpoints [
            get "/" (Response.ofPlainText ("Hello world" ));
            get "/spotify" htmlHandler
            get "/spotify/login" htmlHandler
        ]
    }
    0