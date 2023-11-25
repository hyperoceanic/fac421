module spotifyTypes

open FSharp.Data

type PlayListsTypes = JsonProvider<"samples/playlists.json">

let getPlayLists payload =
    PlayListsTypes.Parse payload

type PlayListTypes = JsonProvider<"samples/playlist.json">

let getPlayList payload =
    PlayListTypes.Parse payload