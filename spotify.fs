module spotify
open System
open System.Web
open Microsoft.AspNetCore.WebUtilities
open System.Text
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization;
open System.Collections.Generic
open System.Net.Http.Headers

let callbackUri = "https://localhost:5001/spotify"

let getLoginURI spotifyClientId =
    let scopes = HttpUtility.UrlEncode "playlist-read-private, playlist-read-collaborative"
    let responseType = "code"
    let showDialog = true
    let builder = new StringBuilder ("https://accounts.spotify.com/authorize")

    builder.Append $"?client_id={spotifyClientId}" |> ignore
    builder.Append $"&scope={scopes}" |> ignore
    builder.Append $"&response_type={responseType}" |> ignore
    builder.Append $"&redirect_uri={callbackUri}" |> ignore
    builder.Append $"&show_dialog={showDialog}" |> ignore
    builder.ToString()

let buildAuth clientId clientSecret =
    $"{clientId}:{clientSecret}"
    |> Encoding.UTF8.GetBytes
    |> Convert.ToBase64String


let getAccessTokenM (auth : string, code : string) =

    let content = Dictionary<string, string>()
    content.Add ("grant_type", "authorization_code")
    content.Add ("code", code)
    content.Add ("redirect_uri", callbackUri)

    task {
        use client = new HttpClient()
        client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue ("Basic", auth)

        let target = "https://accounts.spotify.com/api/token"
        let httpContent = FormUrlEncodedContent content

        let! response = client.PostAsync(target, httpContent)
        return! response.Content.ReadAsStringAsync()
    }
    |> Async.AwaitTask
    |> Async.RunSynchronously
