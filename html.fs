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

let deviceFragment (device : spotifyTypes.DevicesTypes.Devicis) =
     let isActive = if device.IsActive then "(Active)" else ""

     Elem.div [] [
         Elem.p [] [Text.raw $"{device.Name} - {device.Type} {isActive}" ]
     ]

let devicesFragment (devices : spotifyTypes.DevicesTypes.Root) =

    let items = devices.Devices
                |> Array.map (fun e -> Elem.li [] [deviceFragment e] )
                |> Array.toList

    Elem.div [] [
        Elem.h1 [] [ Text.raw "Devices found" ]
        Elem.ul [] items
    ]

let albumView (album : PlayListTypes.Album) =
    Elem.div [] [
        Elem.p []  [Text.raw album.Name]
        Elem.div [HX.target "this" ] [
        Elem.button [
                HX.get $"/play/{album.Uri}";
                HX.swap "outerHTML"

            ] [Text.raw "Play Album"]
        ]

        Elem.a [Attr.href $"https://api.spotify.com/v1/me/player/play"] []
        Elem.img [ Attr.src album.Images[0].Url; Attr.height "200" ]
    ]

let trackView (track : PlayListTypes.Item)  =
    Elem.div [] [
        Elem.div [] [ Text.raw $"{track.Track.Name} - {track.Track.Album.Name}"]
        albumView track.Track.Album
    ]

let playlistView (playlist : PlayListTypes.Root) =
    let tracks = playlist.Tracks.Items
                 |> Array.map (fun e -> trackView e)
                 |> Array.toList

    Elem.div [] [
         Elem.h3 [] [Text.raw playlist.Name]
         Elem.div [Attr.id playlist.Id] [
             Elem.p [] [Text.raw $"Tracks: {playlist.Tracks.Items.Length}"]
             Elem.ul [] tracks
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
    let devices =
        Elem.div [Attr.id "devices"] [
        Elem.button [
            HX.get "/devices";
            HX.target "#devices" ;
            HX.swap "outerHTML"

        ] [Text.raw "Load Devices"]
    ]

    let playlists =
        Elem.div [Attr.id "playlists"] [
            Elem.button [
                HX.get "/playlists";
                HX.target "#playlists" ;
                HX.swap "outerHTML"

            ] [Text.raw "load Playlists"]
        ]

    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        script_htmx
        Elem.body [] [
            Elem.h1 [] [ Text.raw "Fac 421 - logged in" ]
            devices
            playlists
        ]
    ]