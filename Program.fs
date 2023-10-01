module main

open Falco
open Falco.Routing
open Falco.HostBuilder

open html
open spotify

let homePageHandler : HttpHandler = fun ctx ->
    Response.ofHtml homePage ctx

webHost [||] {
    endpoints [
        get "/" homePageHandler
    ]
}