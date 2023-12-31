module spotify

open FsHttp
let getSpotifyAuthToken =
    let result =
        http {
            GET @"https://reqres.in/api/users?page=2&delay=3"
        }
        |> Request.send
        |> Response.toJson
        |> fun json -> json?page.ToString()
    result


// open SpotifyAPI.Web let getLoginURI =
//     let env =  Configuration.GetSpotifyAppConfig
//     let callbackUrl = Uri("https://localhost:5001/spotify/redirect")
//     let scopes = [Scopes.PlaylistReadPrivate;  Scopes.PlaylistReadCollaborative] |> Array.ofSeq
//     let request = LoginRequest( callbackUrl, env.spotify_client_id, LoginRequest.ResponseType.Code)
//     request.Scope <- scopes
//     request.ToUri()

// /// Returns the access credentials for the web site


// /// <summary>Log the user in and get back the user token</summary>
// let getAuthToken env code =
//     let client = OAuthClient()
//     let originatingUri = Uri("https://localhost:5001/spotify/redirect")

//     let request = AuthorizationCodeTokenRequest (
//         env.spotify_client_id,
//         env.spotify_client_secret,
//         code,
//         originatingUri
//     )

//     let getResponse () = async {
//         let! response = client.RequestToken(request) |> Async.AwaitTask
//         return response
//      }

//     let result = getResponse() |> Async.RunSynchronously
//     result

// let getSpotify (refreshHandler, spotify_code) =
//     let env = GetSpotifyAppConfig
//     let auth_response = getAuthToken env spotify_code

//     let authenticator = AuthorizationCodeAuthenticator (
//         env.spotify_client_id,
//         env.spotify_client_secret,
//         auth_response
//     )

//     Event.add refreshHandler authenticator.TokenRefreshed

//     let config = SpotifyClientConfig
//                     .CreateDefault()
//                     .WithToken(auth_response.AccessToken)
//                     .WithAuthenticator (authenticator)


//     SpotifyClient(auth_response.AccessToken);

// let getUserProfile (spotify : SpotifyClient) =
//     async {
//         return! spotify.UserProfile.Current() |> Async.AwaitTask
//     } |> Async.RunSynchronously

// let getPlayLists (spotify : SpotifyClient) =
//     let request = PlaylistCurrentUsersRequest(Limit=5, Offset=0)
//     async {
//         return! spotify.Playlists.CurrentUsers(request) |> Async.AwaitTask
//     } |> Async.RunSynchronously
