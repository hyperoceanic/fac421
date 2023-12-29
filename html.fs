module html

open Falco.Markup
open HTMX
open spotifyTypes

let script_htmx =
    Elem.script [ Attr.src "https://unpkg.com/htmx.org@1.9.2";
    Attr.integrity "sha384-L6OqL9pRWyyFU3+/bjdSri+iIphTN/bvYyM37tICVyOJkWZLpP2vGn6VUEXgzg6h";
    Attr.crossorigin "anonymous" ] []

let styles =
      Elem.link [Attr.rel "stylesheet"; Attr.href "https://cdn.jsdelivr.net/npm/@picocss/pico@1/css/pico.min.css"]

let loginPage loginUrl =
    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        script_htmx
        styles
        Elem.body [Attr.class' "container-fluid"] [
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
    Elem.article [] [
        Elem.div [HX.target "this" ] [
        Elem.button [
                HX.get $"/play/{album.Uri}";
                HX.swap "outerHTML"

            ] [Text.raw $"Play '{album.Artists[0].Name} - {album.Name}'"]
        ]
        Elem.a [ HX.get $"/play/{album.Uri}"; Attr.title $"Play - '{album.Artists[0].Name} - {album.Name}'"] [
        Elem.img [Attr.src album.Images[0].Url; Attr.height "200"; ]
        ]
    ]

let trackView (track : PlayListTypes.Item)  =
    Elem.div [] [
        Elem.div [] [ Text.raw $"{track.Track.Name} - {track.Track.Artists[0].Name}"]
        albumView track.Track.Album
    ]

let playlistAlbumsView  (playlist : PlayListTypes.Root) =
    let albums = playlist.Tracks.Items
                 |> Array.toList
                 |> List.map (fun x -> x.Track.Album)
                 |> List.distinctBy (fun x -> x.Id)
                 |> List.map (fun x -> albumView x)

    Elem.div [] [
         Elem.h3 [] [Text.raw $"{playlist.Name} Albums"]
         Elem.div [Attr.id playlist.Id] [
             Elem.p [] [Text.raw $"Tracks: {playlist.Tracks.Items.Length}"]
             Elem.ul [] albums
        ]
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
    Elem.article [] [
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
            Elem.button [
                HX.get $"/playlistAlbums/{playListsItem.Id}";
                HX.swap "outerHTML"
            ] [Text.raw "Playlist Albums"]

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

            ] [Text.raw "Get Playlists"]
        ]

    Elem.html [ Attr.lang "en" ] [
        Elem.head [] []
        script_htmx
        styles
        Elem.body [Attr.class' "container-fluid"] [
            Elem.h1 [] [ Text.raw "Fac 421 - logged in" ]
            devices
            playlists
        ]
    ]