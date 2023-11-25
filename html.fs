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

let playlist (playlist : PlayListTypes.Root) =
    Elem.div [] [
         Elem.h3 [] [Text.raw playlist.Name]
         Elem.div [Attr.id playlist.Id] [
             Elem.p [] [Text.raw $"Tracks: {playlist.Tracks.Items.Length}"]
        ]
    ]

let playList (playlist : PlayListsTypes.Item) =
    Elem.div [] [
        Elem.h3 [] [Text.raw playlist.Name]
        Elem.div [Attr.id playlist.Id] [
            Elem.button [
                HX.get $"/playlist/{playlist.Id}";
                HX.swap "outerHTML"

            ] [Text.raw "Playlists"]
        ]
    ]

let playlistsItem (playListsItem : spotifyTypes.PlayListsTypes.Item) =
    Elem.div [HX.target "this" ] [
        Elem.h3 [] [Text.raw playListsItem.Name]
        Elem.div [  ] [
            Elem.button [
                HX.get $"/playlist/{playListsItem.Id}";
                HX.swap "outerHTML"
            ] [Text.raw "Playlists"]
        ]
    ]

let playlistsFragment (playlists : spotifyTypes.PlayListsTypes.Root) =

    let items = playlists.Items
                |> Array.map (fun e -> Elem.li [] [playlistsItem e] )
                |> Array.toList

    Elem.div [] [
    Elem.h1 [] [ Text.raw "Playlists found" ]
    Elem.ul [] items

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