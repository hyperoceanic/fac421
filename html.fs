module html

open Falco.Markup

let htmx =
    Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2";
    Attr.integrity "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h";
    Attr.crossorigin "anonymous" ] []

let loginPage loginUrl =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        htmx
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421" ]
            Elem.a [Attr.href loginUrl] [Text.raw "Login to Spotify"]
        ]
    ]

let playlistsFragment content =
    Elem.div [] [
        Elem.h1 [] [ Text.raw "Playlists found" ]
        Elem.p [] [Text.raw content]
    ]

let spotifyPage code =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        htmx
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421 - logged in" ]
            Elem.p [] [Text.raw $"The code is {code}"]
            Elem.div [] [
                Elem.button [ Attr.create "hx-get" "/playlists"

                ] [Text.raw "Playlists"]
            ]



        ]
    ]