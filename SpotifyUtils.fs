module SpotifyUtils

open Configuration
open SpotifyAPI.Web

let getToken env =
    let client = OAuthClient()
    let request = ClientCredentialsRequest (env.spotify_client_id, env.spotify_client_secret)

    let getToken = async {
            return! client.RequestToken request |> Async.AwaitTask
    }

    getToken |> Async.RunSynchronously
