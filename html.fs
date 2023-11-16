module html

open Falco.Markup
open HTMX
open spotifyTypes

let script_htmx =
    Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2";
    Attr.integrity "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h";
    Attr.crossorigin "anonymous" ] []

let loginPage loginUrl =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        script_htmx
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421" ]
            Elem.a [Attr.href loginUrl] [Text.raw "Login to Spotify"]
        ]
    ]

let getPlaylistFragment (item : PlayListResultProvider.PlayListResult) =
    let z = item.Items[0]
    Elem.div [] [Text.raw z.Name]

let playlistsFragment (playlists : PlayListResultProvider.PlayListResult) =

    let elemFrag (x : PlayListResultProvider.Item array) =
        x |> Array.map (fun e -> Elem.p [] [Text.raw e.Name] )

    let items = elemFrag playlists.Items
                |> Array.toList

    Elem.div [] [
    Elem.h1 [] [ Text.raw "Playlists found" ]
    Elem.p [] [Text.raw playlists.Href]
    Elem.div [] items

    ]

let spotifyPage code =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        script_htmx
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421 - logged in" ]
            Elem.p [] [Text.raw $"The code is {code}"]
            Elem.div [Attr.id "playlists"] [
                Elem.button [
                    HX.get "/playlists";
                    HX.target "#playlists" ;
                    HX.swap "outerHTML"

                ] [Text.raw "Playlists"]
            ]



        ]
    ]