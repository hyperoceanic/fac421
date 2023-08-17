module Configuration

open DotEnv.Core

type SpotifyAppConfig = {
    spotify_client_id : string;
    spotify_client_secret: string
}
let GetSpotifyAppConfig : SpotifyAppConfig =
    let loader = EnvLoader().Load();
    let reader =loader.CreateReader()
    let spotify_client_id = reader["spotify_client_id"];
    let spotify_client_secret = reader["spotify_client_secret"];

    {
        spotify_client_id = spotify_client_id;
        spotify_client_secret = spotify_client_secret
    }
