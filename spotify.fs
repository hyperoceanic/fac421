module spotify
open System
open System.Net.Http.Json
open System.Web
open System.Text
open System.Net.Http
open System.Collections.Generic
open System.Net.Http.Headers
open spotifyTypes

let callbackUri = "https://localhost:5001/spotify"

let getLoginURI spotifyClientId =

    let builder = UriBuilder "https://accounts.spotify.com/authorize"
    let query = HttpUtility.ParseQueryString(builder.Query)
    query["client_id"] <- spotifyClientId
    query["scope"] <- "playlist-read-private playlist-read-collaborative user-read-playback-state user-modify-playback-state user-read-currently-playing"
    query["response_type"] <- "code"
    query["redirect_uri"] <- callbackUri
    query["show_dialog"] <- true.ToString()

    $"{builder}?{query}"

let buildAuth clientId clientSecret =
    $"{clientId}:{clientSecret}"
        |> Encoding.UTF8.GetBytes
        |> Convert.ToBase64String

let requestAccessToken (auth : string, code : string) =

    let content = dict [
        "grant_type", "authorization_code";
        "code", code;
        "redirect_uri", callbackUri
    ]

    task {
        use client = new HttpClient()
        client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue ("Basic", auth)

        let target = "https://accounts.spotify.com/api/token"
        let httpContent = new FormUrlEncodedContent (content)
        let! response = client.PostAsync(target, httpContent)
        return! response.Content.ReadAsStringAsync()
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously

let getDevices accessToken  =
    let endpoint = "https://api.spotify.com/v1/me/player/devices"
    use client = new HttpClient()
    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue ("Bearer", accessToken)

    task {
        let! response = client.GetAsync endpoint
        let! payload = response.Content.ReadAsStringAsync()
        return spotifyTypes.getDevicesList payload
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously


let getPlaylists accessToken  =
    let endpoint = "https://api.spotify.com/v1/me/playlists"
    use client = new HttpClient()
    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue ("Bearer", accessToken)

    task {
        let! response = client.GetAsync endpoint
        let! payload = response.Content.ReadAsStringAsync()
        return spotifyTypes.getPlayLists payload
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously


let getPlaylist accessToken Id =
    let endpoint = $"https://api.spotify.com/v1/playlists/{Id}"
    use client = new HttpClient()

    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue ("Bearer", accessToken)

    task {
        let! response = client.GetAsync endpoint
        let! payload = response.Content.ReadAsStringAsync()
        return spotifyTypes.getPlayList payload
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously

type Playable = {context_uri : string; position_ms : int}

let playAlbum accessToken Id =
    let endpoint = $"https://api.spotify.com/v1/me/player/play"
    use client = new HttpClient()

    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", accessToken)

    let jsonContent = JsonContent.Create {context_uri = Id; position_ms = 0 }

    task {
        let! response = client.PutAsync (endpoint, jsonContent)
        return! response.Content.ReadAsStringAsync()
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously

