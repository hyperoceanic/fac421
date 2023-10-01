module fac421.Program

open Falco
open Falco.Routing
open Falco.HostBuilder

open html
open spotify

let homePageHandler : HttpHandler = fun ctx ->
    Response.ofHtml (loginPage getLoginURI) ctx

let redirectPageHandler : HttpHandler = fun ctx ->
    Response.ofHtml spotifyPage ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
        get "/spotify" redirectPageHandler
    ]
}