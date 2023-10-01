module fac421.Home

open Falco
open System
open Microsoft.AspNetCore.Http

open Falco.Markup

open SpotifyUtils
open Cache
open System.Diagnostics


// let loginPart =
//     let uri = getLoginURI
//     Elem.div [] [
//         Elem.a [Attr.href (uri.ToString())] [Text.raw "Spotify Login"]
//         Elem.p [] [Text.raw (uri.ToString())]
//     ]

// let spotifyPart spotify =
//     let userProfile = getUserProfile spotify
//     let playlists = getPlayLists spotify
//     Elem.div [] [
//         Elem.h2 [] [Text.raw userProfile.DisplayName]

//         showUserProfile userProfile
//         showPlaylists playlists
//     ]

// let bar env cache =
//     let x = getClientCredentials (env)
//     saveCode (cache, x.AccessToken) |> ignore
//     Some (x.AccessToken)

// let getAccessToken ctx env =
//     let cache = getCache ctx
//     let cached_spotify_code = getCode cache
//     match cached_spotify_code with
//         | Some value -> Some(value)
//         | _ -> bar env cache

