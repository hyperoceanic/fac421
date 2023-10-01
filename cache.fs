module cache

open System
open System.Diagnostics
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Caching.Memory


let private key = "spotify_code"

let getCache (ctx : HttpContext) =
    ctx.RequestServices.GetService(typeof<IMemoryCache>) :?> MemoryCache

let saveCode (cache : MemoryCache, code) =
    cache.Set (key, code)


let getCode (cache : MemoryCache) : Option<string> =
    let (found, value) = cache.TryGetValue (key)
    match found with
    | true -> Some(value :?> string)
    | false -> None