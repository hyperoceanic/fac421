module spotifyTypes

open FSharp.Data

type PlayListResultProvider = JsonProvider<"samples/playlist.json", RootName = "PlayListResult">

let getPlayListResult (payload:string)  =
    PlayListResultProvider.Parse(payload)