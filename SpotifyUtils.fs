module SpotifyUtils

open System
open Configuration
open SpotifyAPI.Web

let getToken env =
    let client = OAuthClient()
    let request = ClientCredentialsRequest (env.spotify_client_id, env.spotify_client_secret)

    async {
            return! client.RequestToken request |> Async.AwaitTask
    }  |> Async.RunSynchronously

let GetLoginUserUri  env =
    let uri: Uri = Uri ("https://localhost:5001/spotify/redirect")
    let scope = [Scopes.UserReadPrivate] |> Array.ofList

    let loginRequest = LoginRequest (uri, env.spotify_client_id, LoginRequest.ResponseType.Token)
    loginRequest.Scope <- scope
    loginRequest.ToUri()

let getLoginUri: string  =
    let env =  Configuration.GetSpotifyAppConfig
    let url = GetLoginUserUri (env)
    url.ToString()

let GetUserProfile (token: string) =
    let spotify = SpotifyClient (token)
    let profile =spotify.UserProfile

    async {
         return! profile.Current() |> Async.AwaitTask
    } |> Async.RunSynchronously
