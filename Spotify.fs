module Spotify

open FSharp.Data
open Configuration

type AccessToken = AccessToken of string

let QuerySpotify url body  =
    let query = async {
        let! html = Http.AsyncRequestString ( url,
            headers = ["Content-Type", "application/x-www-form-urlencoded"],
            body = TextRequest body)

        return html
    }

    let result = query |> Async.RunSynchronously
    JsonValue.Parse result

let LogInApp configuration =
    let url = "https://accounts.spotify.com/api/token"
    let body = $"grant_type=client_credentials&client_id={configuration.spotify_client_id}&client_secret={configuration.spotify_client_secret}"

    let result = QuerySpotify url body
    let access_token = result["access_token"].InnerText()
    AccessToken access_token
